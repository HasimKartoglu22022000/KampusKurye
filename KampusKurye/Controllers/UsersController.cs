using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using KampusKurye.DbContexts;
using KampusKurye.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KampusKurye.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly PasswordHasher<UsersModel> _hasher;

        public UsersController(AppDbContext db)
        {
            _db = db;
            _hasher = new PasswordHasher<UsersModel>();
        }

        public IActionResult Index()
        {
            return View();
        }

        // ==========================
        //  KAYIT OL (REGISTER)
        // ==========================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsersModel model, string confirmPassword)
        {
            if (model == null) return BadRequest();

            // Sunucu tarafı temel doğrulama
            if (string.IsNullOrWhiteSpace(model.user_firstname) ||
                string.IsNullOrWhiteSpace(model.user_lastname) ||
                string.IsNullOrWhiteSpace(model.user_email) ||
                string.IsNullOrWhiteSpace(model.user_password) ||
                string.IsNullOrWhiteSpace(model.user_name))
            {
                ModelState.AddModelError(string.Empty, "Lütfen zorunlu alanları doldurun.");
            }

            if (model.user_password != confirmPassword)
            {
                ModelState.AddModelError(nameof(confirmPassword), "Şifreler eşleşmiyor.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // E-posta benzersizliği kontrolü
            if (await _db.users.AnyAsync(u => u.user_email == model.user_email))
            {
                ModelState.AddModelError(nameof(model.user_email), "Bu e-posta zaten kayıtlı.");
                return View(model);
            }

            // Kullanıcı adı benzersizliği (istersen)
            if (await _db.users.AnyAsync(u => u.user_name == model.user_name))
            {
                ModelState.AddModelError(nameof(model.user_name), "Bu kullanıcı adı zaten kullanılıyor.");
                return View(model);
            }

            // Varsayılan profil resmi (zorunlu değil ama DB Not Null ise işe yarar)
            if (string.IsNullOrWhiteSpace(model.user_img_url))
            {
                model.user_img_url = "~/images/user.jpg";
            }

            // Hazırlık: GUID, rol, şifre hash
            model.user_guid = Guid.NewGuid();
            model.user_role = 0; // varsayılan rol, ihtiyaca göre değiştir
            model.user_password = _hasher.HashPassword(model, model.user_password ?? string.Empty);

            _db.users.Add(model);
            await _db.SaveChangesAsync();

            TempData["CreateSuccess"] = "Kullanıcı başarıyla oluşturuldu.";
            return RedirectToAction("Login", "Users");
        }

        // ==========================
        //  GİRİŞ (LOGIN)
        // ==========================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UsersModel model, bool rememberMe)
        {
            if (model == null) return BadRequest();

            if (string.IsNullOrWhiteSpace(model.user_name) ||
                string.IsNullOrWhiteSpace(model.user_password))
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı ve şifre zorunludur.");
                return View(model);
            }

            // Kullanıcıyı kullanıcı adına göre bul
            var user = await _db.users.FirstOrDefaultAsync(u => u.user_name == model.user_name);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(model);
            }

            // Şifre doğrulama (hash ile)
            var result = _hasher.VerifyHashedPassword(user, user.user_password, model.user_password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(model);
            }

            // Claims oluştur
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.user_id.ToString()),
                new Claim(ClaimTypes.Name, user.user_name ?? string.Empty),
                new Claim(ClaimTypes.Email, user.user_email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.user_role.ToString())
            };

            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe  // "Beni hatırla"
            };

            // Cookie ile oturum aç
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);

            // Girişten sonra ana sayfaya
            return RedirectToAction("Index", "Home");
        }

        // ==========================
        //  ÇIKIŞ (LOGOUT)
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Users");
        }
    }
}
