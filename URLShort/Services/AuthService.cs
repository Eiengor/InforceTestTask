using URLShort.Models;
using URLShort.Repositories.Interface;

namespace URLShort.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUserNameAsync(username);

            if (user == null)
                return null;

            if (user.PasswordHash == HashPassword(password))
                return user;
            //if (user.PasswordHash == password)
            //    return user;
            return null;
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            var existingUser = await _userRepository.GetByUserNameAsync(username);
            if (existingUser != null)
                return false;

            var newUser = new User
            {
                Username = username,
                PasswordHash = HashPassword(password)
            };

            await _userRepository.AddAsync(newUser);
            return true;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
