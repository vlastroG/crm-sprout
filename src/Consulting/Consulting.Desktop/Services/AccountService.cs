using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Windows.Controls;

using Consulting.Models;
using Consulting.Models.Exceptions;

using Newtonsoft.Json;

namespace Consulting.Desktop.Services {
    public class AccountService {
        private readonly IHttpClientFactory _httpClientFactory;


        public AccountService(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }


        public string Token { get; private set; } = string.Empty;


        /// <exception cref="ServerNotResponseException"></exception>
        public async Task<bool> Login(string email, PasswordBox passwordBox) {
            if(string.IsNullOrWhiteSpace(email)) {
                throw new ArgumentException($"'{nameof(email)}' cannot be null or whitespace.", nameof(email));
            }
            if(passwordBox is null) { throw new ArgumentNullException(nameof(passwordBox)); }

            using HttpClient client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("userName", email);
            client.DefaultRequestHeaders.Add("password", passwordBox.Password);
            var request = new HttpRequestMessage(HttpMethod.Post, Helpers.Constants.LoginUri);
            HttpResponseMessage response;
            try {
                response = await client.SendAsync(request);
            } catch(HttpRequestException) {
                throw new ServerNotResponseException();
            }
            var content = await response.Content.ReadAsStringAsync();

            try {
                var userToken = JsonConvert.DeserializeObject<AuthResponse>(content);
                Token = userToken?.AuthToken ?? string.Empty;
                return true;
            } catch(JsonReaderException) {
                Token = string.Empty;
                return false;
            }
        }

        public void Logout() {
            Token = string.Empty;
        }


        /// <exception cref="ServerNotResponseException"></exception>
        public async Task<bool> Register(string email, PasswordBox passwordBox) {
            if(string.IsNullOrWhiteSpace(email)) {
                throw new ArgumentException($"'{nameof(email)}' cannot be null or whitespace.", nameof(email));
            }
            if(passwordBox is null) { throw new ArgumentNullException(nameof(passwordBox)); }

            using HttpClient client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("email", email);
            client.DefaultRequestHeaders.Add("password", passwordBox.Password);
            var request = new HttpRequestMessage(HttpMethod.Post, Helpers.Constants.RegisterUri);
            HttpResponseMessage response;
            try {
                response = await client.SendAsync(request);
            } catch(HttpRequestException) {
                throw new ServerNotResponseException();
            }
            return response.IsSuccessStatusCode;
        }


        public string GetUserName() {
            if(!string.IsNullOrWhiteSpace(Token)) {
                return new JwtSecurityToken(Token).Subject;
            } else {
                return string.Empty;
            }
        }


        public UserRoles GetUserRole() {
            if(!string.IsNullOrWhiteSpace(Token)) {
                if(new JwtSecurityToken(Token).Claims.Contains(new Claim("admin_access", "true"), new ClaimsComparer())) {
                    return UserRoles.Admin;
                } else {
                    return UserRoles.User;
                }
            } else {
                return UserRoles.Anonym;
            }
        }
    }


    public enum UserRoles {
        Anonym,
        User,
        Admin
    }
}
