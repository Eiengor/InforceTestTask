using System;
using URLShort.Models;
using URLShort.Repositories.Interface;
using URLShort.Context;
using Microsoft.EntityFrameworkCore;

namespace URLShort.Repositories.Concreate
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByUserNameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Username.ToLower() == username.ToLower());
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user == null)
                return;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
