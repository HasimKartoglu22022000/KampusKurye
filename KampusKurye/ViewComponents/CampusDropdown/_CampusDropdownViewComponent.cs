using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KampusKurye.DbContexts;
using KampusKurye.Models;

namespace KampusKurye.ViewComponents
{
    public class CampusDropdownViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;

        public CampusDropdownViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var colleges = await _db.Colleges
                                    .OrderBy(c => c.college_name)
                                    .AsNoTracking()
                                    .ToListAsync();
            return View(colleges);
        }
    }
}