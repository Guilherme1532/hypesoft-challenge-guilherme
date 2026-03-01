using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries.Dashboard;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Handlers.Dashboard;

public class GetDashboardSummaryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    private readonly IDashboardRepository _dashboard;

    public GetDashboardSummaryHandler(IDashboardRepository dashboard)
    {
        _dashboard = dashboard;
    }

    public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken ct)
    {
        var summary = await _dashboard.GetSummaryAsync(ct);

        return new DashboardSummaryDto(
            summary.TotalProducts,
            summary.TotalStockValue,
            summary.LowStockProducts.Select(x => new LowStockProductDto(x.Id, x.Name, x.StockQuantity)).ToList(),
            summary.ProductsByCategory.Select(x => new ProductsByCategoryDto(x.CategoryId, x.Count)).ToList()
        );
    }
}