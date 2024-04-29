using System.Text;

using Consulting.API.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Consulting.API.Auth {
    /// <summary>
    /// https://fullstackmark.com/post/13/jwt-authentication-with-aspnet-core-2-web-api-angular-5-net-core-identity-and-facebook-login
    /// </summary>
    internal static class AuthenticationServiceConfig {
        internal static void ConfigureAuthentication(
            this IServiceCollection services,
            ConfigurationManager configuration) {
            var key = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Contacts:Key"]
                            ?? throw new ArgumentException("Add key to user secrets 256 bits long")));

            var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));

            services.Configure<JwtIssuerOptions>(options => {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters() {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,

                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.TokenValidationParameters = tokenValidationParameters;
                options.SaveToken = true;
            });

            services.AddAuthorization(options => {
                options.AddPolicy(Helpers.Constants.Strings.AuthPolicy.AdminPolicy, policyBuilder
                    => policyBuilder.RequireClaim(
                        Helpers.Constants.Strings.JwtClaimIdentifiers.Admin,
                        Helpers.Constants.Strings.JwtClaims.HasAdminAccess));
            });
        }
    }
}
