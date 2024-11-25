using FitnessApp.Data.Models;
using FitnessApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Data.Repository.Contracts;
using FitnessApp.Data.Repository;
using FitnessApp.Services.Data.Contracts;
using FitnessApp.Services.Data;

namespace FitnessApp.Web.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<IInstructorService, InstructorService>();
            services.AddScoped<ILicenseGeneratorService, LicenseGeneratorService>();
            services.AddScoped<IFitnessClassService, FitnessClassService>();
            services.AddHostedService<FitnessClassStatusUpdateService>();

            return services;
        }

        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("FitnessConnection");
            services.AddDbContext<FitnessAppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IRepository, Repository>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }

        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<FitnessAppDbContext>();

            return services;
        }
    }
}
