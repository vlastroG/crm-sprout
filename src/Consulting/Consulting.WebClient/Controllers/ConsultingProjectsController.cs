using System.Net;

using Consulting.Models;
using Consulting.WebClient.Helpers;
using Consulting.WebClient.Models;
using Consulting.WebClient.Services;

using Microsoft.AspNetCore.Mvc;

namespace Consulting.WebClient.Controllers {
    public class ConsultingProjectsController : AuthBaseController {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FormFileConverter _formFileConverter;

        public ConsultingProjectsController(IHttpClientFactory httpClientFactory, FormFileConverter formFileConverter) {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _formFileConverter = formFileConverter ?? throw new ArgumentNullException(nameof(formFileConverter));
        }



        [HttpGet]
        public async Task<IActionResult> Index() {
            using HttpClient client = _httpClientFactory.CreateClient();
            var consultingProjects = await client.GetFromJsonAsync<IEnumerable<ConsultingProject>>(
                Constants.ConsultingProjectsUri);

            return View(consultingProjects);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id) {
            if(id is null) {
                return NotFound();
            }
            using HttpClient client = _httpClientFactory.CreateClient();
            var consultingProject = await client.GetFromJsonAsync<ConsultingProject>(
                Constants.ConsultingProjectsUri + id);
            if(consultingProject is null) {
                return NotFound();
            }

            return View(consultingProject);
        }

        [HttpGet]
        public IActionResult Create() {
            if(HttpContext.Session.IsAdminUser()) {
                return View(new ProjectViewModel());
            } else {
                return RedirectToAccessDenied();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,Description,Photo")] ProjectViewModel consultingProjectViewModel) {
            if(HttpContext.Session.IsAdminUser()) {
                if(ModelState.IsValid) {
                    using HttpClient client = _httpClientFactory.CreateClient();
                    AddAuthenticationHeader(client);
                    var project = new ConsultingProject() {
                        Description = consultingProjectViewModel.Description,
                        Name = consultingProjectViewModel.Name,
                        Photo = await _formFileConverter.ConvertToByteArray(consultingProjectViewModel.Photo)
                    };

                    var response = await client.PostAsJsonAsync(
                        Constants.ConsultingProjectsUri + Constants.Create, project);
                    switch(response.StatusCode) {
                        case HttpStatusCode.OK:
                            return RedirectToAction(nameof(Index));
                        case HttpStatusCode.Forbidden:
                            return RedirectToAccessDenied();
                        case HttpStatusCode.Unauthorized:
                            return RedirectToLogin();
                        default: {
                            ModelState.AddModelError(
                                nameof(ProjectViewModel.Photo),
                                "Image must be jpeg 225x400 px no greater than 128 KB");
                            return View(consultingProjectViewModel);
                        }
                    }
                }
                return View(consultingProjectViewModel);
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
                var consultingProject = await client.GetFromJsonAsync<ConsultingProject>(
                    Constants.ConsultingProjectsUri + id);
                if(consultingProject is null) {
                    return NotFound();
                }
                var projectViewModel = new ProjectViewModel() {
                    Id = consultingProject.Id,
                    Name = consultingProject.Name,
                    Description = consultingProject.Description,
                    ExistPhoto = Convert.ToBase64String(consultingProject.Photo ?? Array.Empty<byte>())
                };
                return View(projectViewModel);
            } else {
                return RedirectToAccessDenied();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,Description,ExistPhoto,Photo")] ProjectViewModel consultingProjectViewModel) {
            if(HttpContext.Session.IsAdminUser()) {
                if(id != consultingProjectViewModel.Id) {
                    return NotFound();
                }

                if(ModelState.IsValid) {
                    using HttpClient client = _httpClientFactory.CreateClient();
                    AddAuthenticationHeader(client);
                    var consultingProject = new ConsultingProject() {
                        Id = consultingProjectViewModel.Id,
                        Description = consultingProjectViewModel.Description,
                        Name = consultingProjectViewModel.Name,
                        Photo = consultingProjectViewModel.Photo is not null
                        ? await _formFileConverter.ConvertToByteArray(consultingProjectViewModel.Photo)
                        : Convert.FromBase64String(consultingProjectViewModel.ExistPhoto ?? string.Empty)
                    };
                    var response = await client.PutAsJsonAsync(
                        Constants.ConsultingProjectsUri + Constants.Update + id, consultingProject);
                    switch(response.StatusCode) {
                        case HttpStatusCode.OK:
                            return RedirectToAction(nameof(Index));
                        case HttpStatusCode.Forbidden:
                            return RedirectToAccessDenied();
                        case HttpStatusCode.Unauthorized:
                            return RedirectToLogin();
                        case HttpStatusCode.NotFound:
                            return NotFound();
                        default: {
                            ModelState.AddModelError(
                                nameof(ProjectViewModel.Photo),
                                "Image must be jpeg 225x400 px no greater than 128 KB");
                            return View(consultingProjectViewModel);
                        }
                    }
                }
                return View(consultingProjectViewModel);
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
                var consultingProject = await client.GetFromJsonAsync<ConsultingProject>(
                    Constants.ConsultingProjectsUri + id);
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
                return response.StatusCode switch {
                    HttpStatusCode.OK => RedirectToAction(nameof(Index)),
                    HttpStatusCode.Forbidden => RedirectToAccessDenied(),
                    HttpStatusCode.Unauthorized => RedirectToLogin(),
                    _ => NotFound()
                };
            } else {
                return RedirectToAccessDenied();
            }
        }
    }
}
