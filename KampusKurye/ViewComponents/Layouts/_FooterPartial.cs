using Microsoft.AspNetCore.Mvc;

namespace KampusKurye.ViewComponents.Layouts
{
    public class _FooterPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
