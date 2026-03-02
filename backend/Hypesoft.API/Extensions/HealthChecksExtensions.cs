namespace Hypesoft.API.Extensions;

public static class HealthChecksExtensions
{
    public static IServiceCollection AddApiHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
        .AddCheck<MongoDbHealthCheck>("mongodb");

        return services;
    }

    public static WebApplication UseApiHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health").AllowAnonymous();
        return app;
    }
}