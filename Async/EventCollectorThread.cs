using Microsoft.EntityFrameworkCore;
using NetService.Repo;
using NetService.Service;
using System.Diagnostics;
#pragma warning disable CA1416 // Validate platform compatibility

namespace NetService.Async
{
    public class EventCollectorThread : ServiceThread
    {
        public string? _logName { get; set; }
        public EventCollectorThread(EventLogRepo repo) : base(repo)
        {
            //Default Log Group is Security. Override by setting _logName.
            _logName = "Security";
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
           
            while (Thread.CurrentThread.ThreadState.Equals(System.Threading.ThreadState.Running));
            {
                System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog(_logName);
                //eventLog.Log = logName;
                //eventLog.EnableRaisingEvents = true;
                eventLog.Source = _logName;
                
                foreach (EventLogEntry entry in eventLog.Entries)
                {
                    Models.EventLog logModel = new Models.EventLog();
                    logModel.Message = entry.Message;
                    logModel.Machine = entry.MachineName;
                    logModel.Source = _logName;
                    try
                    {
                        this.Repository.Add(logModel);

                    }
                    catch (Exception ex)
                    {
              
                        Console.WriteLine(ex.ToString()); 

                    }
                    //.Entry(logModel).State = EntityState.Modified;
                }
                this.Repository.SaveChanges();



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
#pragma warning restore CA1416 // Validate platform compatibility
