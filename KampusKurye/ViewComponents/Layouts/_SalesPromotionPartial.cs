using Microsoft.AspNetCore.Mvc;

namespace KampusKurye.ViewComponents.Layouts
{
    public class _SalesPromotionPartial: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
