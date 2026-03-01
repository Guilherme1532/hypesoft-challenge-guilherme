using Hypesoft.Infrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Hypesoft.API.Extensions;

public class MongoDbHealthCheck : IHealthCheck
{
    private readonly HypesoftDbContext _db;

    public MongoDbHealthCheck(HypesoftDbContext db)
    {
        _db = db;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var canConnect = await _db.Database.CanConnectAsync(cancellationToken);

            return canConnect
                ? HealthCheckResult.Healthy("MongoDB connection OK")
                : HealthCheckResult.Unhealthy("MongoDB connection failed");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("MongoDB exception", ex);
        }
    }
}