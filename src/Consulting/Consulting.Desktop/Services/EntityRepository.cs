using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using Consulting.Desktop.Helpers;
using Consulting.Models;
using Consulting.Models.Exceptions;

namespace Consulting.Desktop.Services {
    public abstract class EntityRepository<T> : IRepository<T> where T : Entity {
        private readonly AccountService _accountService;
        private readonly IHttpClientFactory _httpClientFactory;

        private protected abstract string Url { get; }

        protected EntityRepository(AccountService accountService, IHttpClientFactory httpClientFactory) {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }


        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UnauthorizedUserException"></exception>
        /// <exception cref="AccessDeniedException"></exception>
        /// <exception cref="ServerNotResponseException"></exception>
        public virtual async Task<bool> Create(T item) {
            if(item is null) { throw new ArgumentNullException(nameof(item)); }

            using HttpClient client = _httpClientFactory.CreateClient();
            AddAuthenticationHeader(client);
            try {
                var response = await client.PostAsJsonAsync(Url + Constants.Create, item);
                return response.StatusCode switch {
                    HttpStatusCode.OK => true,
                    HttpStatusCode.Unauthorized => throw new UnauthorizedUserException(),
                    HttpStatusCode.Forbidden => throw new AccessDeniedException(),
                    _ => false
                };
            } catch(HttpRequestException) {
                throw new ServerNotResponseException();
            }
        }

        /// <exception cref="UnauthorizedUserException"></exception>
        /// <exception cref="AccessDeniedException"></exception>
        /// <exception cref="ServerNotResponseException"></exception>
        public virtual async Task<IEnumerable<T>> Get() {
            using HttpClient client = _httpClientFactory.CreateClient();
            AddAuthenticationHeader(client);
            try {
                var response = await client.GetAsync(Url);
                return response.StatusCode switch {
                    HttpStatusCode.OK => await response.Content.ReadFromJsonAsync<IEnumerable<T>>() ?? Array.Empty<T>(),
                    HttpStatusCode.Unauthorized => throw new UnauthorizedUserException(),
                    _ => throw new AccessDeniedException()
                };
            } catch(HttpRequestException) {
                throw new ServerNotResponseException();
            }
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UnauthorizedUserException"></exception>
        /// <exception cref="AccessDeniedException"></exception>
        /// <exception cref="ServerNotResponseException"></exception>
        public virtual async Task<bool> Update(T item) {
            if(item is null) { throw new ArgumentNullException(nameof(item)); }

            using HttpClient client = _httpClientFactory.CreateClient();
            AddAuthenticationHeader(client);
            try {
                var response = await client.PutAsJsonAsync(Url + Constants.Update + item.Id, item);
                return response.StatusCode switch {
                    HttpStatusCode.OK => true,
                    HttpStatusCode.Unauthorized => throw new UnauthorizedUserException(),
                    HttpStatusCode.Forbidden => throw new AccessDeniedException(),
                    _ => false
                };
            } catch(HttpRequestException) {
                throw new ServerNotResponseException();
            }
        }

        /// <exception cref="UnauthorizedUserException"></exception>
        /// <exception cref="AccessDeniedException"></exception>
        /// <exception cref="ServerNotResponseException"></exception>
        public virtual async Task<bool> Delete(int id) {
            using HttpClient client = _httpClientFactory.CreateClient();
            AddAuthenticationHeader(client);
            try {
                var response = await client.DeleteAsync(Url + Constants.Delete + id);
                return response.StatusCode switch {
                    HttpStatusCode.OK => true,
                    HttpStatusCode.Unauthorized => throw new UnauthorizedUserException(),
                    HttpStatusCode.Forbidden => throw new AccessDeniedException(),
                    _ => false
                };
            } catch(HttpRequestException) {
                throw new ServerNotResponseException();
            }
        }


        private void AddAuthenticationHeader(HttpClient client) {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accountService.Token);
        }
    }
}
