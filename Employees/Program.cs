using Employees.Business;
using Employees.Common.Interfaces;
using Employees.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Employees
{
    internal static class Program
    {
        private static void Main()
        {
            var host = Host.CreateDefaultBuilder()
               
                .ConfigureServices(services =>
                {
                    services.AddData()
                        .AddServices();
                })
                .Build();
            
            var app = host.Services.GetService<IEmployeeService>();
            
            app?.Run();

            
        }
    }
}
