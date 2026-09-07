using Users.Infrastructure.Initialization;

namespace Users.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            await scope.ServiceProvider
                .GetRequiredService<DatabaseInitializer>()
                .InitializeAsync();
        }
    }
}
