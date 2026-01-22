using Microsoft.EntityFrameworkCore;
using URLShort.Context;
using URLShort.Models;
using URLShort.Repositories.Interface;

namespace URLShort.Repositories.Concreate
{
    public class UrlShortenerRepository : IUrlShortenerRepository
    {
        private readonly AppDbContext _context;

        public UrlShortenerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UrlShortener?> GetByIdAsync(int id)
        {
            return await _context.UrlShorteners
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UrlShortener?> GetByShortenedUrlAsync(string shortenedUrl)
        {
            return await _context.UrlShorteners
                .FirstOrDefaultAsync(u =>
                    u.ShortenedUrl.ToLower() == shortenedUrl.ToLower());
        }

        public async Task<List<UrlShortener>> GetAllAsync()
        {
            return await _context.UrlShorteners
                .ToListAsync();
        }

        public async Task AddAsync(UrlShortener urlShortener)
        {
            urlShortener.ShortenedUrl = ShortenUrl(urlShortener.OriginalUrl);
            _context.UrlShorteners.Add(urlShortener);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return;

            _context.UrlShorteners.Remove(entity);
            await _context.SaveChangesAsync();
        }

        private string ShortenUrl(string originalUrl)
        {
            var hash = originalUrl.GetHashCode();
            return Convert.ToBase64String(BitConverter.GetBytes(hash))
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}
