using MauiPlusSqliteApp.Data;
using MauiPlusSqliteApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MauiPlusSqliteApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //for DbContext
            builder.Services.AddDbContext<TodoRepository>(options =>
            {
                var pathDb = Path.Combine(FileSystem.AppDataDirectory, "Todo.db");
                options.UseSqlite($"Data Source={pathDb}");
            });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            //for init DbContext
            var app = builder.Build();
            using(var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TodoRepository>();
                context.Database.EnsureCreated();
                if(!context.Todolist.Any())
                {
                    context.Todolist.AddRange(
                        new Todo { Title = "StandUp"},
                        new Todo { Title = "Relax" } );
                    context.SaveChanges();
                }
            }

            return app;
        }
    }
}
