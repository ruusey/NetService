using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using NetService.Async;
using NetService.Models;
using NetService.Repo;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
#pragma warning disable CA1416 // Validate platform compatibility
namespace NetService.Service
{
    public class EventReaderService : BackgroundService
    {
        public static volatile Dictionary<String, HashSet<EventLogEntry>> LOGS = new Dictionary<String, HashSet<EventLogEntry>>();
        public static Collection<String> LOG_NAMES = new Collection<String>() { "Application", "System", "Security"};
        private HashSet<Thread> threads = new HashSet<Thread>();


        public override Task ExecuteTask => base.ExecuteTask;

        public EventReaderService(IServiceProvider services) 
            
        {
            Services = services;
           
           
            Console.WriteLine("EventReaderServuce constructed successfully. Beginning Collection for "+ threads.Count+" log Groups");

            //this.beginCollection();

        }

        public IServiceProvider Services { get; }


        public async Task DoWorkAsync(CancellationToken token)
        {

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IScopedService>();

                await scopedProcessingService.DoWorkAsync(token);
            }

 
            
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this.DoWorkAsync(stoppingToken);

        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.ExecuteAsync(cancellationToken);
            //return Task.CompletedTask;
        }

       
    }
}
#pragma warning restore CA1416 // Validate platform compatibility

