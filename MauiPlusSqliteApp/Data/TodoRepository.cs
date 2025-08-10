using MauiPlusSqliteApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MauiPlusSqliteApp.Data
{
    public class TodoRepository : DbContext
    {
        public DbSet<Todo> Todolist { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var pathDb = Path.Combine(FileSystem.AppDataDirectory, "Todo.db");
                optionsBuilder.UseSqlite($"Filename={pathDb}");
            }
        }
    }
}
