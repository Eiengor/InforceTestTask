using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace URLShort.Pages
{
    public class ShortUrlInfoModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private const string SessionKeyUserId = "UserId";
        private const string SessionKeyUsername = "Username";

        public ShortUrlInfoModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public bool IsLoggedIn { get; set; }
        public UrlInfoViewModel? UrlInfo { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetInt32(SessionKeyUserId);
            var username = HttpContext.Session.GetString(SessionKeyUsername);

            if (!userId.HasValue || string.IsNullOrEmpty(username))
            {
                IsLoggedIn = false;
                return Page();
            }

            IsLoggedIn = true;

            if (!id.HasValue)
            {
                return RedirectToPage("/ShortUrlTable");
            }
            var apiUrl = $"https://localhost:7122/api/urlshortener/{id.Value}";
            try
            {
                var response = await _httpClient.GetFromJsonAsync<UrlInfoViewModel>(apiUrl);
                UrlInfo = response;
            }
            catch
            {
                UrlInfo = null;
            }

            return Page();
        }
    }

    public class UrlInfoViewModel
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = null!;
        public string ShortenedUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
    }
}
