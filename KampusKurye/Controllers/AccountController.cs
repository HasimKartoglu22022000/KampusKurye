using KampusKurye.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KampusKurye.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = new UsersModel
            {
                user_name = username,
                user_role = 0
            };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.user_name),
                new Claim(ClaimTypes.Role, user.user_role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                  CookieAuthenticationDefaults.AuthenticationScheme,
                  new ClaimsPrincipal(claimsIdentity));

                  return RedirectToAction("Index", "Home");
            }
    }
}
