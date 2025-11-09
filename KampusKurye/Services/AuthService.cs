using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KampusKurye.DbContexts;
using KampusKurye.Models;
using Microsoft.AspNetCore.Http;

namespace KampusKurye.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _http;
        private readonly PasswordHasher<UsersModel> _hasher;

        public AuthService(AppDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
            _hasher = new PasswordHasher<UsersModel>();
        }

        public async Task<bool> SignInAsync(string userOrEmail, string password)
        {
            var user = await _db.users
                .FirstOrDefaultAsync(u => u.user_email == userOrEmail || u.user_name == userOrEmail);

            if (user == null) return false;

            var verify = _hasher.VerifyHashedPassword(user, user.user_password ?? "", password);
            if (verify == PasswordVerificationResult.Failed)
            {
                if (user.user_password != password) return false;

                user.user_password = _hasher.HashPassword(user, password);
                _db.Update(user);
                await _db.SaveChangesAsync();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.user_id.ToString()),
                new Claim(ClaimTypes.Name, user.user_name ?? ""),
                new Claim(ClaimTypes.Email, user.user_email ?? "")
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
            await _http.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return true;
        }

        public async Task SignOutAsync()
        {
            if (_http.HttpContext != null)
                await _http.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<UsersModel?> GetCurrentUserAsync()
        {
            var ctx = _http.HttpContext;
            if (ctx == null || !ctx.User.Identity!.IsAuthenticated) return null;

            var idClaim = ctx.User.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim == null) return null;
            if (!int.TryParse(idClaim.Value, out var userId)) return null;

            return await _db.users.FindAsync(userId);
        }
    }
}