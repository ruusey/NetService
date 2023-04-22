using Microsoft.EntityFrameworkCore;
using NetService.Repo;
using NetService.Service;
using System.Diagnostics;

namespace NetService.Async
{
    public class EventCollectorThread : ServiceThread
    {
        public string? _logName { get; set; }
        public EventCollectorThread(EventLogRepo repo) : base(repo)
        {
            
            Repository = repo;
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void RunThread()
        {
          

            while (true)
            {
                System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog(_logName);
                //eventLog.Log = logName;
                eventLog.Source = _logName;
                
                foreach (EventLogEntry entry in eventLog.Entries)
                {
                    Models.EventLog logModel = new Models.EventLog();
                    logModel.Message = entry.Message;
                    logModel.Machine = entry.MachineName;
                    try
                    {
                        this.Repository.Add(logModel);

                    }
                    catch (Exception ex)
                    {
              
                       

                    }
                    //.Entry(logModel).State = EntityState.Modified;
                }
                this.Repository.SaveChangesAsync();



                try
                {
                    eventLog.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    Console.WriteLine("Collection Thread Sleeping...");
                    Thread.Sleep(10000);
                   
                }
            }
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}
