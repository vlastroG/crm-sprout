using Consulting.WebClient.Services;

namespace Consulting.WebClient {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddRazorPages();
            builder.Services.AddSingleton<FormFileConverter>();

            var app = builder.Build();

            if(!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=ConsultingTasks}/{action=Create}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
