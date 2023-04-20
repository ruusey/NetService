using Microsoft.EntityFrameworkCore;
using NetService.Models;

namespace NetService.Repo
{
    public class TodoRepo : DbContext
    {
        public TodoRepo(DbContextOptions<TodoRepo> options)
       : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();

        public DbSet<Todo> TodoItems { get; set; } = null!;

    }
}
