using Consulting.Models;
using Consulting.WebClient.Helpers;
using Consulting.WebClient.Models;

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
        public async Task<IActionResult> Create([Bind("Id,CreatorName,CreatorEmail,Description,CompanyServiceId,StatusId")] TaskEditorViewModel taskViewModel) {
            if(ModelState.IsValid) {
                using HttpClient client = _httpClientFactory.CreateClient();
                var consultingTask = new ConsultingTask() {
                    CreationDate = DateTime.Now,
                    CreatorName = taskViewModel.CreatorName,
                    CreatorEmail = taskViewModel.CreatorEmail,
                    Description = taskViewModel.Description,
                    CompanyService = new CompanyService() { Id = taskViewModel.CompanyServiceId },
                    Status = new ConsultingTaskStatus() { Id = taskViewModel.StatusId }
                };
                var response = await client.PostAsJsonAsync(Constants.ConsultingTasksUri + Constants.Create, consultingTask);
                var text = await response.Content.ReadAsStringAsync();
                if(response.IsSuccessStatusCode) {
                    return RedirectToAction(nameof(Create));
                } else {
                    return View(taskViewModel);
                }
            }
            return View(taskViewModel);
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
                var vm = new TaskEditorViewModel() {
                    ConsultingTaskId = consultingTask.Id,
                    StatusId = consultingTask.Status!.Id,
                    CreationDate = consultingTask.CreationDate,
                    CreatorName = consultingTask.CreatorName,
                    CreatorEmail = consultingTask.CreatorEmail,
                    CompanyService = consultingTask.CompanyService!.Name,
                    Description = consultingTask.Description
                };
                return View(vm);
            } else {
                return RedirectToAccessDenied();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConsultingTaskId,CreatorName,CreatorEmail,Description,CompanyService,StatusId")] TaskEditorViewModel viewModel) {
            if(HttpContext.Session.IsAdminUser()) {
                if(id != viewModel?.ConsultingTaskId) {
                    return NotFound();
                }

                if(ModelState.IsValid) {
                    using HttpClient client = _httpClientFactory.CreateClient();
                    AddAuthenticationHeader(client);
                    var task = await client.GetFromJsonAsync<ConsultingTask>(Constants.ConsultingTasksUri + viewModel.ConsultingTaskId);
                    if(task is null) { return NotFound(); }
                    task.Status = new ConsultingTaskStatus() { Id = viewModel.StatusId };

                    var response = await client.PutAsJsonAsync(Constants.ConsultingTasksUri + Constants.Update + id, task);
                    if(response.IsSuccessStatusCode) {
                        return RedirectToAction(nameof(Index));
                    } else if(response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
                        return RedirectToAccessDenied();
                    } else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                        return RedirectToLogin();
                    } else {
                        return NotFound();
                    }
                }
                return View(viewModel);
            } else {
                return RedirectToAccessDenied();
            }
        }
    }
}
