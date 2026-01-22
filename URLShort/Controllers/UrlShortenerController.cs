using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URLShort.Models;
using URLShort.Repositories.Interface;

namespace URLShort.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlShortenerController : ControllerBase
    {
        private readonly IUrlShortenerRepository _urlShortenerRepository;
        public UrlShortenerController(IUrlShortenerRepository urlShortenerRepository)
        {
            _urlShortenerRepository = urlShortenerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<UrlShortener>>> GetAll()
        {
            var urls = await _urlShortenerRepository.GetAllAsync();
            return Ok(urls);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UrlShortener>> GetById(int id)
        {
            var urlShortener = await _urlShortenerRepository.GetByIdAsync(id);
            if (urlShortener == null)
                return NotFound();

            return Ok(urlShortener);
        }

        [HttpGet("by-shortened-url/{shortenedUrl}")]
        public async Task<ActionResult<UrlShortener>> GetByShortenedUrl(string shortenedUrl)
        {
            var urlShortener = await _urlShortenerRepository.GetByShortenedUrlAsync(shortenedUrl);
            if (urlShortener == null)
                return NotFound();

            return Ok(urlShortener);
        }

        [HttpPost]
        public async Task<ActionResult<UrlShortener>> Create(UrlShortener urlShortener)
        {
            await _urlShortenerRepository.AddAsync(urlShortener);

            return CreatedAtAction(
                nameof(GetById),
                new { id = urlShortener.Id },
                urlShortener
            );
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var urlShortener = await _urlShortenerRepository.GetByIdAsync(id);

            if (urlShortener == null)
                return NotFound();

            await _urlShortenerRepository.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}
