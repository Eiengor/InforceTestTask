using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using URLShort.Services;

namespace URLShort.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAuthService _authService;
        private const string SessionKeyUserId = "UserId";
        private const string SessionKeyUsername = "Username";

        public IndexModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public bool IsLoggedIn { get; set; }
        public string? CurrentUsername { get; set; }
        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public void OnGet()
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetInt32(SessionKeyUserId);
            var username = HttpContext.Session.GetString(SessionKeyUsername);

            if (userId.HasValue && !string.IsNullOrEmpty(username))
            {
                IsLoggedIn = true;
                CurrentUsername = username;
            }
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Username and password are required.";
                return Page();
            }

            var user = await _authService.LoginAsync(Username, Password);

            if (user == null)
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }

            // Store user info in session
            HttpContext.Session.SetInt32(SessionKeyUserId, user.Id);
            HttpContext.Session.SetString(SessionKeyUsername, user.Username);

            SuccessMessage = "Login successful!";
            IsLoggedIn = true;
            CurrentUsername = user.Username;

            return Page();
        }

        public IActionResult OnPostLogout()
        {
            // Clear session
            HttpContext.Session.Remove(SessionKeyUserId);
            HttpContext.Session.Remove(SessionKeyUsername);

            SuccessMessage = "Logged out successfully.";
            return RedirectToPage();
        }
    }
}
