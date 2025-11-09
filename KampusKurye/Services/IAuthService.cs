using KampusKurye.Models;
namespace KampusKurye.Services
{
    public interface IAuthService
    {
        Task<bool> SignInAsync(string userOrEmail, string password);
        Task SignOutAsync();
        Task<UsersModel?> GetCurrentUserAsync();
    }
}
