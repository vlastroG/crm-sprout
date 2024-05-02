using Microsoft.AspNetCore.Mvc;

namespace Consulting.WebClient.Controllers {
    public class ConsultingProjectsController : AuthBaseController {
        public IActionResult Index() {
            return View();
        }
    }
}
