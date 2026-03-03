using Hypesoft.Infrastructure.Data;

namespace Hypesoft.API.Extensions;

public static class DatabaseSeedExtensions
{
    public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");
        var db = scope.ServiceProvider.GetRequiredService<HypesoftDbContext>();

        try
        {
            await DbSeeder.SeedAsync(db);
            logger.LogInformation("Database seed checked successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to seed database.");
            throw;
        }

        return app;
    }
}
