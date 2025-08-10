using MauiPlusSqliteApp.Data;
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

            builder.Services.AddDbContext<TodoRepository>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TodoRepository>();
                context.Database.EnsureCreated();
            }

            return app;
        }

        static void CreateDb(MauiApp app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<TodoRepository>();
                DbInitialize.Initialize(context);
            }
            catch(Exception ex)
            {
                //TODO: logger.LogError(ex, "Failed to create the Db")
            }
        }
    }
}
