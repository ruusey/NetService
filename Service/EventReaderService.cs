using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
#pragma warning disable CA1416 // Validate platform compatibility
namespace NetService.Service
{
    public class EventReaderService : IEventReaderService
    {
        public static volatile Dictionary<String, HashSet<EventLogEntry>> LOGS = new Dictionary<String, HashSet<EventLogEntry>>();

        private HashSet<Thread> threads = new HashSet<Thread>();
        public EventReaderService(Collection<String> logNames) 
        {
            foreach(String logName in logNames)
            {
                Console.WriteLine("Creating Collection Thread for Log Group "+logName);

                EventLog eventLog = new EventLog(logName);
                eventLog.Log = logName;
                eventLog.Source = logName;

                if (!LOGS.ContainsKey(logName))
                {
                    LOGS.Add(logName, new HashSet<EventLogEntry>());
                            
                }
                Thread readerThread = new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;

                    while (true)
                    {
                        HashSet<EventLogEntry> currLogs = LOGS.GetValueOrDefault(logName, new HashSet<EventLogEntry>());
                       
                        foreach (EventLogEntry entry in eventLog.Entries)
                        {
                            currLogs.Add(entry);
                        }
                        if (LOGS.ContainsKey(logName) && LOGS.GetValueOrDefault(logName, new HashSet<EventLogEntry>()).Count == 0)
                        {
                            LOGS.TryAdd(logName, currLogs);
                        }
                        try
                        {
                            //eventLog.Clear();
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
                   
                   


                });
                readerThread.Name = logName;
                this.threads.Add(readerThread);
            }
            Console.WriteLine("EventReaderServuce constructed successfully. Beginning Collection for "+ threads.Count+" log Groups");

            this.beginCollection();

        }
        public void beginCollection()
        {
            foreach (Thread readerThread in threads)
            {
                Console.WriteLine("Starting sys log collection for "+readerThread.Name);
                readerThread.Start();
            }
        }

        public HashSet<EventLogEntry> getEntrysByLogGroup(String logGroupName, Int32 limit)
        {
            HashSet<EventLogEntry> res = new HashSet<EventLogEntry>();
            HashSet<EventLogEntry> currLogs = LOGS.GetValueOrDefault(logGroupName, new HashSet<EventLogEntry>());
            if(currLogs.Count > 0) {
                if (currLogs.Count <= limit)
                {
                    limit = currLogs.Count;
                }
                for(Int32 i = 0; i < limit; i++)
                {
                    res.Add(currLogs.ElementAt(i));
                }
               
            }
            return res;
        }
    }
}
#pragma warning restore CA1416 // Validate platform compatibility

