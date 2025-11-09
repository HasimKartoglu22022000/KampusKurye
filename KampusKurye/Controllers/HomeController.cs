using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KampusKurye.Models;
using KampusKurye.DbContexts;

namespace KampusKurye.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // GET: / or /Home/Index
        public async Task<IActionResult> Index()
        {
            var restaurants = await _db.Restaurants
                .AsNoTracking()
                .Include(r => r.College)
                .OrderBy(r => r.restaurant_name)
                .ToListAsync();

            return View(restaurants);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}