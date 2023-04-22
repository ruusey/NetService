using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NetService.Async;
using NetService.Repo;
using NetService.Service;
using System.Collections.ObjectModel;
namespace NetService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            RunCollector(host);

            host.Run();
        }
        private static void RunCollector(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context1 = services.GetService<EventReaderService>();
                    context1.StartAsync(CancellationToken.None);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to start EventCollection Service " + ex.Message);
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
    }
}



