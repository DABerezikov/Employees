using Employees.Business.Services;
using Employees.Common.Interfaces;
using Employees.Data.Entities;
using Employees.Data.Repositories;
using Employees.Data.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IRepository<Employee>, EmployeeRepository>()
                .AddTransient<IEmployeeService, EmployeeService>();
                

            return services;
        }
    }
}
