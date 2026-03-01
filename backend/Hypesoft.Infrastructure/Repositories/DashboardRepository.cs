using Hypesoft.Domain.Repositories;
using Hypesoft.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hypesoft.Infrastructure.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly HypesoftDbContext _db;

    public DashboardRepository(HypesoftDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardSummary> GetSummaryAsync(CancellationToken ct = default)
    {
        var totalProducts = await _db.Products.AsNoTracking().LongCountAsync(ct);

        // TotalStockValue = sum(price * stock)
        // fallback caso provider não traduza a multiplicação
        decimal totalStockValue;
        try
        {
            totalStockValue = await _db.Products.AsNoTracking()
                .Select(p => p.Price * p.StockQuantity)
                .SumAsync(ct);
        }
        catch
        {
            var values = await _db.Products.AsNoTracking()
                .Select(p => new { p.Price, p.StockQuantity })
                .ToListAsync(ct);

            totalStockValue = values.Sum(x => x.Price * x.StockQuantity);
        }

        var lowStock = await _db.Products.AsNoTracking()
            .Where(p => p.StockQuantity < 10)
            .OrderBy(p => p.StockQuantity)
            .Take(10)
            .Select(p => new LowStockItem(p.Id, p.Name, p.StockQuantity))
            .ToListAsync(ct);

        var categoryIds = await _db.Products.AsNoTracking()
            .Select(p => p.CategoryId)
            .Where(id => id != null && id != "")
            .ToListAsync(ct);

        var byCategory = categoryIds
            .GroupBy(id => id)
            .Select(g => new ProductsByCategoryItem(g.Key, g.LongCount()))
            .ToList();

        return new DashboardSummary(
            totalProducts,
            totalStockValue,
            lowStock,
            byCategory
        );
    }
}