using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

namespace TodoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Todo> TodoItems { get; set; }
    }
}
