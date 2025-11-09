using System.Linq;
using System.Security.Claims;
using KampusKurye.DbContexts;
using KampusKurye.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KampusKurye.Controllers
{
    public class AdminOrdersController : Controller
    {
        private readonly AppDbContext _db;

        public AdminOrdersController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Users");

            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            int.TryParse(roleClaim, out int userRole);

            // 🔒 Sadece user_role >= 1 olanlar erişebilsin
            if (userRole < 1)
                return Forbid();

            // 🔹 Order + User + Restaurant join edip ViewModel'e map ediyoruz
            var model = await
                (from o in _db.order
                 join r in _db.Restaurants
                    on o.restaurant_id equals r.restaurant_id into rj
                 from r in rj.DefaultIfEmpty()   // restoran null olabilir diye left join
                 join u in _db.users
                    on o.user_id equals u.user_id into uj
                 from u in uj.DefaultIfEmpty()   // user null olabilir diye left join
                 orderby o.order_created_at descending
                 select new AdminOrderListItemViewModel
                 {
                     OrderId = o.order_id,
                     OrderNumber = o.order_number ?? "",
                     RestaurantName = r != null ? r.restaurant_name : "(Restoran yok)",
                     CustomerName = u != null ? (u.user_firstname + " " + u.user_lastname) : "(Kullanıcı yok)",
                     TotalPrice = o.order_total_price,
                     Status = o.order_status ?? 0,
                     CreatedAt = o.order_created_at,
                     DeliveryAddress = o.order_delivery_address,
                     CustomerNote = o.order_customer_note
                 })
                .ToListAsync();

            return View(model); // ✅ Artık List<AdminOrderListItemViewModel> gidiyor
        }
    }
}
