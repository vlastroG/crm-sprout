
using Consulting.API.Auth;
using Consulting.API.Data;
using Consulting.API.Data.Repos;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Consulting.API {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ConsultingDbContext>(
                options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("ConsultingDbContext")
                ?? throw new InvalidOperationException("Connection string not found")));

            builder.Services.AddSingleton<JwtFactory>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddRepositories();
            builder.Services.AddIdentityCore<ApplicationUser>(o => {
                o.Password.RequireDigit = false;
                o.Password.RequiredLength = 8;
                o.Password.RequireLowercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;

                o.SignIn.RequireConfirmedAccount = false;
                o.SignIn.RequireConfirmedEmail = false;
                o.SignIn.RequireConfirmedPhoneNumber = false;
            }).AddEntityFrameworkStores<ConsultingDbContext>();
            builder.Services.AddSwaggerGen(c => {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
                    Description = "JWT Authorization header using the Bearer scheme(Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.ConfigureAuthentication(builder.Configuration);

            var app = builder.Build();

            if(app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            if(app.Environment.IsDevelopment()) {
                using(var scope = app.Services.CreateScope()) {
                    var db = scope.ServiceProvider.GetRequiredService<ConsultingDbContext>();
                    db.Database.Migrate();
                    db.Dispose();
                }
            }

            app.Run();
        }
    }
}
