using Microsoft.AspNetCore.Mvc;

namespace Consulting.WebClient.Controllers {
    public class CompanyServicesController : AuthBaseController {
        public IActionResult Index() {
            return View();
        }
    }
}
