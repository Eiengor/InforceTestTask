using URLShort.Models;

namespace URLShort.Services
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string password);
    }
}
