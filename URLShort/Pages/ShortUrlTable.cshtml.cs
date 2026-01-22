using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace URLShort.Pages
{
    public class ShortUrlTableModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private const string SessionKeyUserId = "UserId";
        private const string SessionKeyUsername = "Username";
        public ShortUrlTableModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public List<ShortUrlViewModel> Urls { get; set; } = new();
        public bool IsLoggedIn { get; set; }
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public async Task OnGetAsync()
        {
            UserId = HttpContext.Session.GetInt32(SessionKeyUserId);
            Username = HttpContext.Session.GetString(SessionKeyUsername);

            if (UserId.HasValue && !string.IsNullOrEmpty(Username))
            {
                IsLoggedIn = true;
            }
            var apiUrl = "https://localhost:7122/api/urlshortener";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<ShortUrlViewModel>>(apiUrl);
                if (response != null)
                {
                    Urls = response;
                }
            }
            catch
            {
                Urls = new List<ShortUrlViewModel>();
            }
        }
    }

    public class ShortUrlViewModel
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = null!;
        public string ShortenedUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
