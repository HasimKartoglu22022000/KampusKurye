using KampusKurye.Helpers;
using KampusKurye.Models;
using Microsoft.AspNetCore.Mvc;

namespace KampusKurye.ViewComponents.Layouts
{
    public class _HeaderPartial : ViewComponent
    {
        private const string CartSessionKey = "Cart";

        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.GetObject<CartModel>(CartSessionKey) ?? new CartModel();

            return View(cart);
        }
    }
}
