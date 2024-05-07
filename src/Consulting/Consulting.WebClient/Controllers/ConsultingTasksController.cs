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
        public async Task<IActionResult> Create([Bind("Id,CreationDate,CreatorName,CreatorEmail,Description,Status,CompanyService")] ConsultingTask consultingTask) {
            if(ModelState.IsValid) {
                using HttpClient client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync(Constants.ConsultingTasksUri + Constants.Create, consultingTask);
                if(response.IsSuccessStatusCode) {
                    return RedirectToAction(nameof(Index));
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
                var statuses = await client.GetFromJsonAsync<IEnumerable<ConsultingTaskStatus>>(Constants.ConsultingTaskStatusesUri);
                HttpContext.Items.Add(Constants.ConsultingTaskStatusesUri, statuses);
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
        public async Task<IActionResult> Edit(int id, [Bind("ConsultingTaskId,StatusId")] TaskEditorViewModel viewModel) {
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
