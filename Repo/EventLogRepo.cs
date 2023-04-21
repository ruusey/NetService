using Microsoft.EntityFrameworkCore;
using NetService.Models;

namespace NetService.Repo
{
    public class EventLogRepo : DbContext
    {
        public EventLogRepo(DbContextOptions<EventLogRepo> options)
       : base(options) { }

        public DbSet<EventLog> Events => Set<EventLog>();

        public DbSet<EventLog> EventItems { get; set; } = null!;

    }
}
