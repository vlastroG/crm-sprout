using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Consulting.Models;

namespace Consulting.WebClient.Helpers {
    internal static class RolesHandler {
        internal static bool IsAdminUser(this ISession session) {
            var encodedToken = GetToken(session);
            if(!string.IsNullOrWhiteSpace(encodedToken)) {
                var token = new JwtSecurityToken(encodedToken);
                return token.Claims.Contains(
                    new Claim(Constants.AdminClaimType, Constants.AdminClaimValue),
                    new ClaimsComparer());
            } else {
                return false;
            }
        }

        private static string GetToken(ISession session) {
            return session.GetString(Constants.TokenName) ?? string.Empty;
        }
    }
}
