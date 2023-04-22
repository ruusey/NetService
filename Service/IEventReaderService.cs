using System.Diagnostics;

namespace NetService.Service
{
    public interface IEventReaderService : IHostedService
    {
        void beginCollection();
        HashSet<EventLogEntry> getEntrysByLogGroup(String logGroupName, Int32 limit);
    }
}
