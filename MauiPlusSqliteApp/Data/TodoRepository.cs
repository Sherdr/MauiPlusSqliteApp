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
                optionsBuilder.UseSqlite($"Filename={pathDb}")
                    .UseSeeding((context, _) =>
                    {
                        var relaxExist = context.Set<Todo>().Any(x => x.Title == "Relax");
                        if (!relaxExist)
                        {
                            context.Set<Todo>().Add(new Todo { Title = "Relax" } );
                        }
                        context.SaveChanges();
                    });
            }
        }
    }
}
