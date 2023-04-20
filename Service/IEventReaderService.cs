using System.Diagnostics;

namespace NetService.Service
{
    public interface IEventReaderService
    {
        void beginCollection();
        HashSet<EventLogEntry> getEntrysByLogGroup(String logGroupName, Int32 limit);
    }
}
