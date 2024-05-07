using Consulting.Models;
using Consulting.WebClient.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace Consulting.WebClient.Controllers {
    public class BlogPostsController : AuthBaseController {
        private readonly IHttpClientFactory _httpClientFactory;

        public BlogPostsController(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }


        [HttpGet]
        public async Task<IActionResult> Index() {
            using HttpClient client = _httpClientFactory.CreateClient();
            var blogPosts = await client.GetFromJsonAsync<IEnumerable<BlogPost>>(Helpers.Constants.BlogPostsUri);

            return View(blogPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id) {
            if(id is null) {
                return NotFound();
            }
            using HttpClient client = _httpClientFactory.CreateClient();
            var blogPost = await client.GetFromJsonAsync<BlogPost>(Helpers.Constants.BlogPostsUri + id);
            if(blogPost is null) {
                return NotFound();
            }

            return View(blogPost);
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
        public async Task<IActionResult> Create([Bind("Id,Name,ContentShort,ContentFull,Photo")] BlogPost blogPost) {
            if(HttpContext.Session.IsAdminUser()) {
                if(ModelState.IsValid) {
                    using HttpClient client = _httpClientFactory.CreateClient();
                    AddAuthenticationHeader(client);
                    var response = await client.PostAsJsonAsync(Constants.BlogPostsUri + Constants.Create, blogPost);
                    if(response.IsSuccessStatusCode) {
                        return RedirectToAction(nameof(Index));
                    } else if(response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
                        return RedirectToAccessDenied();
                    } else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                        return RedirectToLogin();
                    } else {
                        return BadRequest();
                    }
                }
                return View(blogPost);
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
                var blogPost = await client.GetFromJsonAsync<BlogPost>(Constants.BlogPostsUri + id);
                if(blogPost is null) {
                    return NotFound();
                }
                return View(blogPost);
            } else {
                return RedirectToAccessDenied();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ContentShort,ContentFull,Photo")] BlogPost blogPost) {
            if(HttpContext.Session.IsAdminUser()) {
                if(id != blogPost.Id) {
                    return NotFound();
                }

                if(ModelState.IsValid) {
                    using HttpClient client = _httpClientFactory.CreateClient();
                    AddAuthenticationHeader(client);
                    var response = await client.PutAsJsonAsync(Constants.BlogPostsUri + Constants.Update + id, blogPost);
                    if(response.IsSuccessStatusCode) {
                        return RedirectToAction(nameof(Index));
                    } else if(response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
                        return RedirectToAccessDenied();
                    } else {
                        return NotFound();
                    }
                }
                return View(blogPost);
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
                var blogPost = await client.GetFromJsonAsync<BlogPost>(Constants.BlogPostsUri + id);
                if(blogPost is null) { return NotFound(); }
                return View(blogPost);
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
                var response = await client.DeleteAsync(Constants.BlogPostsUri + Constants.Delete + id);
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
