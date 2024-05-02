using Microsoft.AspNetCore.Mvc;

namespace Consulting.WebClient.Controllers {
    public class ConsultingTasksController : AuthBaseController {
        public IActionResult Index() {
            return View();
        }
    }
}
