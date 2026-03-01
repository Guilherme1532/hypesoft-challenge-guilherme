namespace Hypesoft.Application.DTOs;

public record DashboardSummaryDto(
    long TotalProducts,
    decimal TotalStockValue,
    IReadOnlyList<LowStockProductDto> LowStockProducts,
    IReadOnlyList<ProductsByCategoryDto> ProductsByCategory
);

public record LowStockProductDto(string Id, string Name, int StockQuantity);

public record ProductsByCategoryDto(string CategoryId, long Count);