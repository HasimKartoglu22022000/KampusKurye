using KampusKurye.DbContexts;
using KampusKurye.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace KampusKurye.Controllers
{
    public class Restaurants : Controller
    {
        private readonly AppDbContext _db;

        public Restaurants(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Restaurants/Index/5
        public async Task<IActionResult> Index(int id)
        {
            // 1) İlgili restoranı çek
            var restaurant = await _db.Restaurants
                .AsNoTracking()
                .Include(r => r.College) // RestaurantModel'de College navigation'ı varsa
                .FirstOrDefaultAsync(r => r.restaurant_id == id);

            if (restaurant == null)
                return NotFound();

            // 2) O restorana ait ürünleri çek
            var products = await _db.products
                .AsNoTracking()
                .Where(p => p.restaurant_id == id)
                .ToListAsync();

            // 3) Tüm kategorileri çek
            var categories = await _db.categories
                .AsNoTracking()
                .OrderBy(c => c.categories_name)
                .ToListAsync();

            // 4) ViewModel'i doldur
            var vm = new RestaurantDetailsViewModel
            {
                Restaurant = restaurant,
                Products = products,
                Categories = categories
            };

            // 5) View'e gönder
            return View(vm);
        }
    }
}