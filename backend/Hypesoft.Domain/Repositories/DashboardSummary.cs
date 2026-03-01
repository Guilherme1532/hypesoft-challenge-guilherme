namespace Hypesoft.Domain.Repositories;

public record DashboardSummary(
    long TotalProducts,
    decimal TotalStockValue,
    IReadOnlyList<LowStockItem> LowStockProducts,
    IReadOnlyList<ProductsByCategoryItem> ProductsByCategory
);

public record LowStockItem(string Id, string Name, int StockQuantity);

public record ProductsByCategoryItem(string CategoryId, long Count);