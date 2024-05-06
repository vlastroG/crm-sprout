using Consulting.Models;
using Consulting.WebClient.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace Consulting.WebClient.Controllers {
    public class ConsultingTasksController : AuthBaseController {
        private readonly IHttpClientFactory _httpClientFactory;

        public ConsultingTasksController(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }


        [HttpGet]
        public async Task<IActionResult> Index() {
            if(HttpContext.Session.IsAdminUser()) {
                using HttpClient client = _httpClientFactory.CreateClient();
                AddAuthenticationHeader(client);
                var consultingTasks = await client.GetFromJsonAsync<IEnumerable<ConsultingTask>>(Helpers.Constants.ConsultingTasksUri);

                return View(consultingTasks);
            } else {
                return RedirectToAccessDenied();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id) {
            if(HttpContext.Session.IsAdminUser()) {
                if(id is null) {
                    return NotFound();
                }
                using HttpClient client = _httpClientFactory.CreateClient();
                AddAuthenticationHeader(client);
                var consultingTask = await client.GetFromJsonAsync<ConsultingTask>(Helpers.Constants.ConsultingTasksUri + id);
                if(consultingTask is null) {
                    return NotFound();
                }

                return View(consultingTask);
            } else {
                return RedirectToAccessDenied();
            }
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreationDate,CreatorName,CreatorEmail,Description,Status,CompanyService")] ConsultingTask consultingTask) {
            if(ModelState.IsValid) {
                using HttpClient client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync(Constants.ConsultingTasksUri + Constants.Create, consultingTask);
                if(response.IsSuccessStatusCode) {
                    return RedirectToAction(nameof(Index));
                } else if(response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
                    return RedirectToAccessDenied();
                } else {
                    return BadRequest();
                }
            }
            return View(consultingTask);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id) {
            if(HttpContext.Session.IsAdminUser()) {
                if(id is null) { return NotFound(); }

                using HttpClient client = _httpClientFactory.CreateClient();
                AddAuthenticationHeader(client);
                var consultingTask = await client.GetFromJsonAsync<ConsultingTask>(Constants.ConsultingTasksUri + id);
                if(consultingTask is null) {
                    return NotFound();
                }
                return View(consultingTask);
            } else {
                return RedirectToAccessDenied();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreationDate,CreatorName,CreatorEmail,Description,Status,CompanyService")] ConsultingTask consultingTask) {
            if(HttpContext.Session.IsAdminUser()) {
                if(id != consultingTask.Id) {
                    return NotFound();
                }

                if(ModelState.IsValid) {
                    using HttpClient client = _httpClientFactory.CreateClient();
                    AddAuthenticationHeader(client);
                    var response = await client.PutAsJsonAsync(Constants.ConsultingTasksUri + Constants.Update + id, consultingTask);
                    if(response.IsSuccessStatusCode) {
                        return RedirectToAction(nameof(Index));
                    } else if(response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
                        return RedirectToAccessDenied();
                    } else {
                        return NotFound();
                    }
                }
                return View(consultingTask);
            } else {
                return RedirectToAccessDenied();
            }
        }
    }
}
