using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static FitnessApp.Web.Extensions.ApplicationBuilderExtensions;
using static FitnessApp.Web.Extensions.ServiceCollectionExtension;

namespace FitnessApp.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationDbContext(builder.Configuration);
            builder.Services.AddApplicationIdentity(builder.Configuration);

            builder.Services.AddLocalization(opt => opt.ResourcesPath = "Resources");
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new CultureInfo[]
                {
        new CultureInfo("bg"),
        new CultureInfo("en")
                };

                options.DefaultRequestCulture = new RequestCulture("bg");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>()
    {
        new CookieRequestCultureProvider()
    };
            });

            builder.Services.AddControllersWithViews(options =>
            {
                //options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            builder.Services.AddApplicationServices();
            builder.Services.AddMemoryCache();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRequestLocalization();
            app.UseStaticFiles();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "FitnessClass Details",
                    pattern: "/FitnessClass/Details/{id}/{information}",
                    defaults: new { Controller = "FitnessClass", Action = "Details" }
                );

                endpoints.MapControllerRoute(
                        name: "areas",
                        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });

            await app.CreateAdminRoleAsync();

            await app.RunAsync();
        }
    }
}
