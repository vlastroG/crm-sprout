#nullable disable

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Consulting.WebClient.Areas.Identity.Pages.Account {
    public class RegisterModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterModel(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }


        public class InputModel {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} должен быть длиной как минимум {2} и максимум {1} символов.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Подтвердите пароль")]
            [Compare("Password", ErrorMessage = "Введенные пароли не совпадают.")]
            public string ConfirmPassword { get; set; }
        }


        public void OnGetAsync(string returnUrl = null) {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null) {
            returnUrl ??= Url.Content("~/");
            if(ModelState.IsValid) {
                using HttpClient client = _httpClientFactory.CreateClient();

                client.DefaultRequestHeaders.Add("email", Input.Email);
                client.DefaultRequestHeaders.Add("password", Input.Password);
                var request = new HttpRequestMessage(HttpMethod.Post, Helpers.Constants.RegisterUri);
                var response = await client.SendAsync(request);
                if(response.IsSuccessStatusCode) {
                    return LocalRedirect(returnUrl);
                } else {
                    ModelState.AddModelError(string.Empty, "Пользователь с таким email уже существует");
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
