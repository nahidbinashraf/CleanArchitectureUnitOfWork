using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnitOfWork.Core.Interfaces;
using UnitOfWork.Core.Models;
using UnitOfWork.Infrastructure.Data;
using UnitOfWork.Infrastructure.Repositories;

namespace UnitOfWork.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = new AppSettings();
            configuration.Bind(appSettings);
            services.AddSingleton(appSettings);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection);
            });

            services.AddScoped<IUnitOfWork, UnitOfWorks>();
            services.AddScoped<IEmployeeRespository, EmployeeRespository>();

            return services;
        }
    }
}
