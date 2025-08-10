using MauiPlusSqliteApp.Models;

namespace MauiPlusSqliteApp.Data
{
    public class DbInitialize
    {
        public static void Initialize(TodoRepository context)
        {
            context.Database.EnsureCreated();
            if(context.Todolist.Any())
            {
                return;
            }
            context.Todolist.AddRange(
                new Todo { Title = "StandUp" },
                new Todo { Title = "Relax" });
            context.SaveChanges();
        }
    }
}
