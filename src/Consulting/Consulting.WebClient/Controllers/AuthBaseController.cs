using System.Net.Http.Headers;

using Consulting.WebClient.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace Consulting.WebClient.Controllers {
    public abstract class AuthBaseController : Controller {
        protected AuthBaseController() {

        }


        private protected RedirectToPageResult RedirectToLogin() {
            HttpContext.Session.Clear();
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        private protected RedirectToPageResult RedirectToAccessDenied() {
            return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
        }

        private protected void AddAuthenticationHeader(HttpClient client) {
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString(Constants.TokenName));
        }
    }
}
