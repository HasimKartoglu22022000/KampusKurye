using KampusKurye.DbContexts;
using KampusKurye.Helpers;
using KampusKurye.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KampusKurye.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private const string CartSessionKey = "Cart";

        public CartController(AppDbContext db)
        {
            _db = db;
        }

        private CartModel GetCart()
        {
            var cart = HttpContext.Session.GetObject<CartModel>(CartSessionKey);
            if (cart == null)
            {
                cart = new CartModel();
                HttpContext.Session.SetObject(CartSessionKey, cart);
            }
            return cart;
        }

        private void SaveCart(CartModel cart)
        {
            HttpContext.Session.SetObject(CartSessionKey, cart);
        }

        // 🔹 Sepet sayfası
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);    // @model CartModel
        }

        // 🔹 Ürün ekleme (Restoran detay + sepet içindeki + butonu)
        [HttpPost]
        public async Task<IActionResult> Add(int id, int quantity = 1)
        {
            var product = await _db.products
                .FirstOrDefaultAsync(p => p.product_id == id);

            if (product == null)
                return NotFound();

            var cart = GetCart();

            cart.AddItem(
                product.product_id,
                product.product_name,
                product.product_price,
                product.product_imgurl,
                quantity
            );

            SaveCart(cart);

            // İstersen önceki sayfaya dönmek için:
            // var referer = Request.Headers["Referer"].ToString();
            // return Redirect(referer);

            return RedirectToAction("Index");
        }

        // 🔹 Adet azaltma (- butonu)
        [HttpPost]
        public IActionResult Decrease(int id)
        {
            var cart = GetCart();
            cart.DecreaseItem(id);   // order_id == id olan item’in quantity’sini azaltır
            SaveCart(cart);

            return RedirectToAction("Index");
        }

        // 🔹 Ürünü tamamen kaldır (Kaldır butonu)
        [HttpPost]
        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            cart.RemoveItem(id);
            SaveCart(cart);

            return RedirectToAction("Index");
        }

        // 🔹 Sepeti tamamen boşalt
        [HttpPost]
        public IActionResult Clear()
        {
            var cart = GetCart();
            cart.Clear();
            SaveCart(cart);

            return RedirectToAction("Index");
        }
    }
}
