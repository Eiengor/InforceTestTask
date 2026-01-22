using URLShort.Models;
namespace URLShort.Repositories.Interface
{
    public interface IUrlShortenerRepository
    {
        Task<UrlShortener?> GetByIdAsync(int id);
        Task<UrlShortener?> GetByShortenedUrlAsync(string shortenedUrl);
        Task<List<UrlShortener>> GetAllAsync();
        Task AddAsync(UrlShortener urlShortener);
        Task DeleteByIdAsync(int id);
    }
}
