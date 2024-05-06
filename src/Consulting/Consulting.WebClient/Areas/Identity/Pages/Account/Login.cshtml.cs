#nullable disable

using System.ComponentModel.DataAnnotations;

using Consulting.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Newtonsoft.Json;

namespace Consulting.WebClient.Areas.Identity.Pages.Account {
    public class LoginModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        [BindProperty]
        public InputModel Input { get; set; }


        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public void OnGetAsync(string returnUrl = null) {
            if(!string.IsNullOrEmpty(ErrorMessage)) {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            HttpContext.Session.Clear();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null) {
            returnUrl ??= Url.Content("~/");

            if(ModelState.IsValid) {
                using HttpClient client = _httpClientFactory.CreateClient();

                client.DefaultRequestHeaders.Add("userName", Input.Email);
                client.DefaultRequestHeaders.Add("password", Input.Password);
                var request = new HttpRequestMessage(HttpMethod.Post, Helpers.Constants.LoginUri);
                var response = await client.SendAsync(request);
                if(response.StatusCode == System.Net.HttpStatusCode.BadRequest) {
                    ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
                    return Page();
                }
                var content = await response.Content.ReadAsStringAsync();

                try {
                    var userToken = JsonConvert.DeserializeObject<AuthResponse>(content);
                    HttpContext.Session.SetString(Helpers.Constants.TokenName, userToken?.AuthToken);
                    return LocalRedirect(returnUrl);
                } catch(JsonReaderException) {
                    ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
                    return Page();
                }
            }
            return Page();
        }
    }
}
