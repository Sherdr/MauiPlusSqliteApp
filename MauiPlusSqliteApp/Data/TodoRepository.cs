using MauiPlusSqliteApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MauiPlusSqliteApp.Data
{
    public class TodoRepository : DbContext
    {
        public DbSet<Todo> Todolist { get; set; }

        public TodoRepository(DbContextOptions<TodoRepository> options) : base(options) { }
    }
}
