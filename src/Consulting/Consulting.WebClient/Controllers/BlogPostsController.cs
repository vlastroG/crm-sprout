using System.Net;

using Consulting.Models;
using Consulting.WebClient.Helpers;
using Consulting.WebClient.Models;
using Consulting.WebClient.Services;

using Microsoft.AspNetCore.Mvc;

namespace Consulting.WebClient.Controllers {
    public class BlogPostsController : AuthBaseController {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FormFileConverter _formFileConverter;

        public BlogPostsController(IHttpClientFactory httpClientFactory, FormFileConverter formFileConverter) {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _formFileConverter = formFileConverter ?? throw new ArgumentNullException(nameof(formFileConverter));
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
                return View(new BlogPostViewModel());
            } else {
                return RedirectToAccessDenied();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,ContentShort,ContentFull,Photo")] BlogPostViewModel blogPostViewModel) {
            if(HttpContext.Session.IsAdminUser()) {
                if(ModelState.IsValid) {
                    using HttpClient client = _httpClientFactory.CreateClient();
                    AddAuthenticationHeader(client);
                    var blogPost = new BlogPost() {
                        Name = blogPostViewModel.Name,
                        ContentShort = blogPostViewModel.ContentShort,
                        ContentFull = blogPostViewModel.ContentFull,
                        Photo = await _formFileConverter.ConvertToByteArray(blogPostViewModel.Photo)
                    };

                    var response = await client.PostAsJsonAsync(Constants.BlogPostsUri + Constants.Create, blogPost);
                    switch(response.StatusCode) {
                        case HttpStatusCode.OK:
                            return RedirectToAction(nameof(Index));
                        case HttpStatusCode.Forbidden:
                            return RedirectToAccessDenied();
                        case HttpStatusCode.Unauthorized:
                            return RedirectToLogin();
                        default: {
                            ModelState.AddModelError(
                                nameof(BlogPostViewModel.Photo),
                                "Image must be jpeg 225x400 px no greater than 128 KB");
                            return View(blogPostViewModel);
                        };
                    };
                };
                return View(blogPostViewModel);
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
                var blogPostViewModel = new BlogPostViewModel() {
                    Id = blogPost.Id,
                    Name = blogPost.Name,
                    ContentShort = blogPost.ContentShort,
                    ContentFull = blogPost.ContentFull,
                    ExistPhoto = Convert.ToBase64String(blogPost.Photo ?? Array.Empty<byte>())
                };
                return View(blogPostViewModel);
            } else {
                return RedirectToAccessDenied();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,ContentShort,ContentFull,ExistPhoto,Photo")] BlogPostViewModel blogPostViewModel) {
            if(HttpContext.Session.IsAdminUser()) {
                if(id != blogPostViewModel.Id) {
                    return NotFound();
                }

                if(ModelState.IsValid) {
                    using HttpClient client = _httpClientFactory.CreateClient();
                    AddAuthenticationHeader(client);
                    var blogPost = new BlogPost() {
                        Id = blogPostViewModel.Id,
                        Name = blogPostViewModel.Name,
                        ContentShort = blogPostViewModel.ContentShort,
                        ContentFull = blogPostViewModel.ContentFull,
                        Photo = blogPostViewModel.Photo is not null
                        ? await _formFileConverter.ConvertToByteArray(blogPostViewModel.Photo)
                        : Convert.FromBase64String(blogPostViewModel.ExistPhoto ?? string.Empty)
                    };
                    var response = await client.PutAsJsonAsync(Constants.BlogPostsUri + Constants.Update + id, blogPost);
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
                                nameof(BlogPostViewModel.Photo),
                                "Image must be jpeg 225x400 px no greater than 128 KB");
                            return View(blogPostViewModel);
                        }
                    }
                }
                return View(blogPostViewModel);
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
                return response.StatusCode switch {
                    HttpStatusCode.OK => RedirectToAction(nameof(Index)),
                    HttpStatusCode.Forbidden => RedirectToAccessDenied(),
                    HttpStatusCode.Unauthorized => RedirectToLogin(),
                    _ => NotFound(),
                };
            } else {
                return RedirectToAccessDenied();
            }
        }
    }
}
