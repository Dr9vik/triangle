using Business_Logic_Layer.Common.Services;
using Business_Logic_Layer.Services;
using Data_Access_Layer.Common.Repositories;
using Data_Access_Layer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Triangle.ConfiguringApps;

namespace Triangle
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            var serviceProvider = new ServiceCollection();
            serviceProvider.AddTransient<IFileRepository, FileRepository>();
            serviceProvider.AddTransient<ITriangleService, TriangleService>();
            serviceProvider.AddSingleton<IConfiguration>(Configuration);
            serviceProvider.AddScoped<Form1>();
            serviceProvider.AddSingleton<ExceptionMiddleware>();


            using (ServiceProvider service = serviceProvider.BuildServiceProvider())
            {
                var form1 = service.GetRequiredService<Form1>();
                Application.Run(form1);
            }
        }
    }
}