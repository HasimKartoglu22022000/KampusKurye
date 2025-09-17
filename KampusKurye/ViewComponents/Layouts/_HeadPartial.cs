using Microsoft.AspNetCore.Mvc;

namespace KampusKurye.ViewComponents.Layouts
{
    public class _HeadPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
