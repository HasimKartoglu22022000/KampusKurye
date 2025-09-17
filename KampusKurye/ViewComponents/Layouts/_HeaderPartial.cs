using Microsoft.AspNetCore.Mvc;

namespace KampusKurye.ViewComponents.Layouts
{
    public class _HeaderPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
