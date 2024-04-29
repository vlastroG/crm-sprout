using System.Security.Claims;

using Consulting.API.Auth;
using Consulting.API.Models;
using Consulting.Models;

using Newtonsoft.Json;

namespace Consulting.API.Helpers {
    internal class Tokens {
        internal static async Task<string> GenerateJwt(
            ClaimsIdentity identity,
            JwtFactory jwtFactory,
            string userName,
            JwtIssuerOptions jwtOptions,
            JsonSerializerSettings serializerSettings) {
            var response = new AuthResponse() {
                Id = identity.Claims.Single(c => c.Type == "id").Value,
                AuthToken = await jwtFactory.GenerateEncodedToken(userName, identity),
                ExpiresInSec = (int) jwtOptions.ValidFor.TotalSeconds
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}
