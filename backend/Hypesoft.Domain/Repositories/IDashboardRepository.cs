namespace Hypesoft.Domain.Repositories;

public interface IDashboardRepository
{
    Task<DashboardSummary> GetSummaryAsync(CancellationToken ct = default);
}