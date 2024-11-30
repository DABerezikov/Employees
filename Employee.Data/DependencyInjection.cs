using Employees.Data.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            services
                .AddSingleton<DbInitializer>();

            return services;
        }
    }
}
