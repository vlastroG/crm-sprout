using Consulting.Models;
using Consulting.WebClient.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace Consulting.WebClient.Controllers {
    public class ConsultingProjectsController : AuthBaseController {
        private readonly IHttpClientFactory _httpClientFactory;

        public ConsultingProjectsController(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }



        [HttpGet]
        public async Task<IActionResult> Index() {
            using HttpClient client = _httpClientFactory.CreateClient();
            var consultingProjects = await client.GetFromJsonAsync<IEnumerable<ConsultingProject>>(Helpers.Constants.ConsultingProjectsUri);

            return View(consultingProjects);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id) {
            if(id is null) {
                return NotFound();
            }
            using HttpClient client = _httpClientFactory.CreateClient();
            var consultingProject = await client.GetFromJsonAsync<ConsultingProject>(Helpers.Constants.ConsultingProjectsUri + id);
            if(consultingProject is null) {
                return NotFound();
            }

            return View(consultingProject);
        }

        [HttpGet]
        public IActionResult Create() {
            if(HttpContext.Session.IsAdminUser()) {
                return View();
            } else {
                return RedirectToAccessDenied();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Photo")] ConsultingProject consultingProject) {
            if(HttpContext.Session.IsAdminUser()) {
                if(ModelState.IsValid) {
                    using HttpClient client = _httpClientFactory.CreateClient();
                    AddAuthenticationHeader(client);
                    var response = await client.PostAsJsonAsync(Constants.ConsultingProjectsUri + Constants.Create, consultingProject);
                    if(response.IsSuccessStatusCode) {
                        return RedirectToAction(nameof(Index));
                    } else if(response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
                        return RedirectToAccessDenied();
                    } else {
                        return BadRequest();
                    }
                }
                return View(consultingProject);
            } else {
                return RedirectToAccessDenied();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id) {
            if(HttpContext.Session.IsAdminUser()) {
                if(id is null) { return NotFound(); }

                using HttpClient client = _httpClientFactory.CreateClient();
                AddAuthenticationHeader(client);
                var consultingProject = await client.GetFromJsonAsync<ConsultingProject>(Constants.ConsultingProjectsUri + id);
                if(consultingProject is null) {
                    return NotFound();
                }
                return View(consultingProject);
            } else {
                return RedirectToAccessDenied();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Photo")] ConsultingProject consultingProject) {
            if(HttpContext.Session.IsAdminUser()) {
                if(id != consultingProject.Id) {
                    return NotFound();
                }

                if(ModelState.IsValid) {
                    using HttpClient client = _httpClientFactory.CreateClient();
                    AddAuthenticationHeader(client);
                    var response = await client.PutAsJsonAsync(Constants.ConsultingProjectsUri + Constants.Update + id, consultingProject);
                    if(response.IsSuccessStatusCode) {
                        return RedirectToAction(nameof(Index));
                    } else if(response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
                        return RedirectToAccessDenied();
                    } else {
                        return NotFound();
                    }
                }
                return View(consultingProject);
            } else {
                return RedirectToAccessDenied();
            }
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id) {
            if(HttpContext.Session.IsAdminUser()) {
                if(id is null) {
                    return NotFound();
                }

                using HttpClient client = _httpClientFactory.CreateClient();
                AddAuthenticationHeader(client);
                var consultingProject = await client.GetFromJsonAsync<ConsultingProject>(Constants.ConsultingProjectsUri + id);
                if(consultingProject is null) { return NotFound(); }
                return View(consultingProject);
            } else {
                return RedirectToAccessDenied();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            if(HttpContext.Session.IsAdminUser()) {
                using HttpClient client = _httpClientFactory.CreateClient();
                AddAuthenticationHeader(client);
                var response = await client.DeleteAsync(Constants.ConsultingProjectsUri + Constants.Delete + id);
                if(response.IsSuccessStatusCode) {
                    return RedirectToAction(nameof(Index));
                } else if(response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
                    return RedirectToAccessDenied();
                } else {
                    return NotFound();
                }
            } else {
                return RedirectToAccessDenied();
            }
        }
    }
}
