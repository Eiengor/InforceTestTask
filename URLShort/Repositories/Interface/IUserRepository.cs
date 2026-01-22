using URLShort.Models;

namespace URLShort.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUserNameAsync(string username);
        Task<List<User>> GetAllAsync();
        Task AddAsync(User user);
        Task DeleteByIdAsync(int id);
    }
}
