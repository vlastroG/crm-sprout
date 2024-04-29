using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

using Consulting.API.Data;
using Consulting.API.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Consulting.API.Auth {
    public class JwtFactory {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<string, ApplicationUser> _applicationUsers;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions, IServiceProvider serviceProvider) {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _applicationUsers = new ConcurrentDictionary<string, ApplicationUser>();
        }


        public async Task<ClaimsIdentity> GenerateClaimsIdentity(string userName, string id) {
            var userClaims = await GetUserClaimsAsync(userName);
            userClaims.Add(new Claim(
                Helpers.Constants.Strings.JwtClaimIdentifiers.Id, id));
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), userClaims);
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity) {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(
                    JwtRegisteredClaimNames.Iat,
                    ToUnixWpochDate(_jwtOptions.IssuedAt).ToString(),
                    ClaimValueTypes.Integer64),
                identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Id),
                identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Admin)
            };

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }


        /// <returns>
        /// Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).
        /// </returns>
        private static long ToUnixWpochDate(DateTime date) {
            return (long) Math.Round(
                (date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options) {
            if(options == null)
                throw new ArgumentNullException(nameof(options));

            if(options.ValidFor <= TimeSpan.Zero) {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if(options.SigningCredentials == null) {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if(options.JtiGenerator == null) {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        private async Task<IList<Claim>> GetUserClaimsAsync(string userName) {
            var user = await GetUserAsync(userName);
            using(var scope = _serviceProvider.CreateScope()) {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                return await userManager.GetClaimsAsync(user);
            }
        }

        private async Task<ApplicationUser> GetUserAsync(string userName) {
            if(_applicationUsers.TryGetValue(userName, out ApplicationUser? user)) {
                return user;
            } else {
                using(var scope = _serviceProvider.CreateScope()) {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    user = await userManager.FindByNameAsync(userName);
                    if(user != null && _applicationUsers.TryAdd(userName, user)) {
                        return user;
                    } else {
                        throw new ArgumentException($"User '{userName}' not found");
                    }
                }
            }
        }
    }
}
