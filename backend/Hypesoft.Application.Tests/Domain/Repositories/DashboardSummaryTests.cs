using FluentAssertions;
using Hypesoft.Domain.Repositories;

namespace Hypesoft.Application.Tests.Domain.Repositories;

public class DashboardSummaryTests
{
    [Fact]
    public void Records_ShouldStoreProvidedValues()
    {
        var lowStock = new List<LowStockItem>
        {
            new("p-1", "Mouse", 3),
            new("p-2", "Teclado", 5)
        };
        var byCategory = new List<ProductsByCategoryItem>
        {
            new("cat-1", 10),
            new("cat-2", 4)
        };

        var summary = new DashboardSummary(
            TotalProducts: 14,
            TotalStockValue: 4567.89m,
            LowStockProducts: lowStock,
            ProductsByCategory: byCategory
        );

        summary.TotalProducts.Should().Be(14);
        summary.TotalStockValue.Should().Be(4567.89m);
        summary.LowStockProducts.Should().HaveCount(2);
        summary.ProductsByCategory.Should().HaveCount(2);
        summary.LowStockProducts[0].Name.Should().Be("Mouse");
        summary.ProductsByCategory[1].CategoryId.Should().Be("cat-2");
    }
}
