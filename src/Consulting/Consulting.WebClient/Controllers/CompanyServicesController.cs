using Consulting.Models;
using Consulting.WebClient.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace Consulting.WebClient.Controllers {
    public class CompanyServicesController : AuthBaseController {
        private readonly IHttpClientFactory _httpClientFactory;

        public CompanyServicesController(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }



        [HttpGet]
        public async Task<IActionResult> Index() {
            using HttpClient client = _httpClientFactory.CreateClient();
            var companyServices = await client.GetFromJsonAsync<IEnumerable<CompanyService>>(Constants.CompanyServicesUri);

            return View(companyServices);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id) {
            if(id is null) {
                return NotFound();
            }
            using HttpClient client = _httpClientFactory.CreateClient();
            var companyService = await client.GetFromJsonAsync<CompanyService>(Constants.CompanyServicesUri + id);
            if(companyService is null) {
                return NotFound();
            }

            return View(companyService);
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
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] CompanyService companyService) {
            if(HttpContext.Session.IsAdminUser()) {
                if(ModelState.IsValid) {
                    using HttpClient client = _httpClientFactory.CreateClient();
                    AddAuthenticationHeader(client);
                    var response = await client.PostAsJsonAsync(Constants.CompanyServicesUri + Constants.Create, companyService);
                    if(response.IsSuccessStatusCode) {
                        return RedirectToAction(nameof(Index));
                    } else if(response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
                        return RedirectToAccessDenied();
                    } else {
                        return BadRequest();
                    }
                }
                return View(companyService);
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
                var companyService = await client.GetFromJsonAsync<CompanyService>(Constants.CompanyServicesUri + id);
                if(companyService is null) {
                    return NotFound();
                }
                return View(companyService);
            } else {
                return RedirectToAccessDenied();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] CompanyService companyService) {
            if(HttpContext.Session.IsAdminUser()) {
                if(id != companyService.Id) {
                    return NotFound();
                }

                if(ModelState.IsValid) {
                    using HttpClient client = _httpClientFactory.CreateClient();
                    AddAuthenticationHeader(client);
                    var response = await client.PutAsJsonAsync(Constants.CompanyServicesUri + Constants.Update + id, companyService);
                    if(response.IsSuccessStatusCode) {
                        return RedirectToAction(nameof(Index));
                    } else if(response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
                        return RedirectToAccessDenied();
                    } else {
                        return NotFound();
                    }
                }
                return View(companyService);
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
                var companyService = await client.GetFromJsonAsync<CompanyService>(Constants.CompanyServicesUri + id);
                if(companyService is null) { return NotFound(); }
                return View(companyService);
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
                var response = await client.DeleteAsync(Constants.CompanyServicesUri + Constants.Delete + id);
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
