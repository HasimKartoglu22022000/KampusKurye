using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KampusKurye.DbContexts;
using KampusKurye.Helpers;
using KampusKurye.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KampusKurye.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _db;
        private const string CartSessionKey = "Cart";

        public OrderController(AppDbContext db)
        {
            _db = db;
        }

        // 🔹 SİPARİŞ OLUŞTURMA (Sepeti DB'ye yaz)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(string order_delivery_address, string order_customer_note)
        {
            // 1) Sepeti session'dan çek
            var cart = HttpContext.Session.GetObject<CartModel>(CartSessionKey) ?? new CartModel();

            if (cart.Items == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            // 2) Sepetteki ilk üründen restoran bilgisini bul
            int? restaurantId = null;

            var firstItem = cart.Items.FirstOrDefault();
            if (firstItem != null)
            {
                // DİKKAT: burada order_id aslında ürün ID'si olarak kullanılıyor
                var product = await _db.products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.product_id == firstItem.order_id);

                if (product != null)
                {
                    restaurantId = product.restaurant_id;
                }
            }

            // Eğer yine de bulunamadıysa, güvenlik için siparişi iptal edelim
            if (restaurantId == null)
            {
                // İstersen TempData ile mesaj da gösterebilirsin
                TempData["Error"] = "Restoran bilgisi bulunamadı, lütfen tekrar deneyin.";
                return RedirectToAction("Index", "Cart");
            }

            // 3) Siparişi oluştur
            var order = new OrderModel
            {
                order_guid = Guid.NewGuid(),
                order_created_at = DateTime.Now,
                order_total_price = cart.TotalPrice,
                order_status = 0, // Bekliyor

                order_delivery_address = order_delivery_address,
                order_customer_note = order_customer_note ?? string.Empty,
                order_company_note = string.Empty,

                // 🔥 ARTIK BOŞ DEĞİL:
                restaurant_id = restaurantId.Value
            };

            // 4) Kullanıcı login ise user_id bağla
            if (User?.Identity?.IsAuthenticated == true)
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (!string.IsNullOrEmpty(email))
                {
                    var user = await _db.users.FirstOrDefaultAsync(u => u.user_email == email);
                    if (user != null)
                    {
                        order.user_id = user.user_id;
                    }
                }
            }

            // 5) Sepetteki her ürünü OrderItem olarak ekle
            foreach (var item in cart.Items)
            {
                order.Items.Add(new OrderItemModel
                {
                    // Bunu ürün ID'si olacak şekilde kendine göre düzelt:
                    product_id = item.order_id,       // eğer gerçekten product_id ise
                    product_name = item.order_name,
                    product_price = item.order_price,
                    product_quantity = item.order_quantity,
                    product_imgurl = item.order_img_url
                });
            }

            _db.order.Add(order);
            await _db.SaveChangesAsync();

            // 6) Sepeti temizle
            HttpContext.Session.SetObject(CartSessionKey, new CartModel());

            // 7) Sipariş detayına yönlendir
            return RedirectToAction("Detail", new { id = order.order_id });
        }

        // 🔹 SİPARİŞ DETAYI (tek sipariş)
        public async Task<IActionResult> Detail(int id)
        {
            var order = await _db.order
                .Include(o => o.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.order_id == id);

            if (order == null)
                return NotFound();

            return View(order); // @model OrdersModel olan bir view yaparsın
        }

        // 🔹 SİPARİŞ LİSTESİ (senin mevcut aksiyonun)
        public async Task<IActionResult> List()
        {
            // İstersen sadece login kullanıcının siparişlerini göster:
            IQueryable<OrderModel> query = _db.order; // OrdersModel tipini kullanıyorsun

            if (User?.Identity?.IsAuthenticated == true)
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var user = await _db.users.FirstOrDefaultAsync(u => u.user_email == email);
                if (user != null)
                {
                    query = query.Where(o => o.user_id == user.user_id);
                }
            }

            var orders = await query
                .AsNoTracking()
                .OrderByDescending(o => o.order_created_at)
                .ToListAsync();

            return View(orders);
        }
    }
}
