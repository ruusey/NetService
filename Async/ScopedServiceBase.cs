using Microsoft.EntityFrameworkCore;
using NetService.Repo;
using NetService.Service;
using NuGet.Common;
using System.Diagnostics;
using System.Threading;

namespace NetService.Async
{
    public sealed class ScopedServiceBase : IScopedService
    {
        private int _executionCount;
        private EventLogRepo _context { get; set; }
        private HashSet<EventCollectorThread> threads = new HashSet<EventCollectorThread>();

        public ScopedServiceBase(EventLogRepo context) { 
            _context = context;
        }
        public async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                foreach (String logName in EventReaderService.LOG_NAMES)
                {
                    EventCollectorThread t = new EventCollectorThread(_context);
                    t._logName = logName;
                    threads.Add(t);
                }

                await this.getThreadTask();
                
               
            }
           
        }

        public Task getThreadTask()
        {
            Task task = new Task(() =>
            {
                foreach (EventCollectorThread readerThread in threads)
                {
                    Console.WriteLine("Starting sys log collection for " + readerThread.ToString);
                    if (!readerThread.IsAlive)
                    {
                        readerThread.RunThread();
                    }
                }

            });
            task.Start();
            
            return task;

           
        }
    }
}
