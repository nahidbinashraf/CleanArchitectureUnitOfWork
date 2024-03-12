using Microsoft.Extensions.DependencyInjection;
using UnitOfWork.Services.Interfaces;
using UnitOfWork.Services.Services;

namespace UnitOfWork.Services.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServiceDI(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            return services;
        }
    }
}
