namespace Consulting.API.Helpers {
    internal static class Constants {
        internal static class Strings {
            internal static class JwtClaimIdentifiers {
                internal const string Id = "id";
                internal const string Admin = "admin_access";
            }

            internal static class JwtClaims {
                internal const string HasAdminAccess = "true";
            }

            internal static class AuthPolicy {
                internal const string AdminPolicy = "Admin";
            }
        }
    }
}
