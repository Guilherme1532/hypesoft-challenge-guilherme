using FluentAssertions;
using Hypesoft.Domain.Entities;

namespace Hypesoft.Application.Tests.Domain.Entities;

public class ProductTests
{
    [Fact]
    public void Constructor_ShouldSetAllFields_WithTrimmedValues()
    {
        var product = new Product(
            "  Mouse  ",
            "  RGB  ",
            120.50m,
            "  cat-1 ",
            8
        );

        product.Name.Should().Be("Mouse");
        product.Description.Should().Be("RGB");
        product.Price.Should().Be(120.50m);
        product.CategoryId.Should().Be("cat-1");
        product.StockQuantity.Should().Be(8);
        product.Id.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsInvalid()
    {
        var act = () => new Product(" ", "d", 10m, "cat-1", 1);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Product name is required.*");
    }

    [Fact]
    public void SetName_ShouldThrow_WhenNameIsNull()
    {
        var product = new Product("Mouse", "d", 10m, "cat-1", 1);

        var act = () => product.SetName(null!);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Product name is required.*");
    }

    [Fact]
    public void SetDescription_ShouldAcceptNull_AndStoreEmptyString()
    {
        var product = new Product("Mouse", "desc", 10m, "cat-1", 1);

        product.SetDescription(null!);

        product.Description.Should().BeEmpty();
    }

    [Fact]
    public void SetPrice_ShouldThrow_WhenNegative()
    {
        var product = new Product("Mouse", "d", 10m, "cat-1", 1);

        var act = () => product.SetPrice(-0.01m);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Price cannot be negative.*");
    }

    [Fact]
    public void SetCategory_ShouldThrow_WhenCategoryIsEmpty()
    {
        var product = new Product("Mouse", "d", 10m, "cat-1", 1);

        var act = () => product.SetCategory(" ");

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Category is required.*");
    }

    [Fact]
    public void SetStock_ShouldThrow_WhenNegative()
    {
        var product = new Product("Mouse", "d", 10m, "cat-1", 1);

        var act = () => product.SetStock(-1);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Stock cannot be negative.*");
    }

    [Theory]
    [InlineData(9, 10, true)]
    [InlineData(10, 10, false)]
    [InlineData(2, 2, false)]
    [InlineData(1, 2, true)]
    public void IsLowStock_ShouldRespectThreshold(int currentStock, int threshold, bool expected)
    {
        var product = new Product("Mouse", "d", 10m, "cat-1", currentStock);

        var isLow = product.IsLowStock(threshold);

        isLow.Should().Be(expected);
    }

    [Fact]
    public void Rehydrate_ShouldKeepProvidedIdAndValues()
    {
        var product = Product.Rehydrate("prod-1", "Teclado", "Mecanico", 299m, "cat-2", 15);

        product.Id.Should().Be("prod-1");
        product.Name.Should().Be("Teclado");
        product.Description.Should().Be("Mecanico");
        product.Price.Should().Be(299m);
        product.CategoryId.Should().Be("cat-2");
        product.StockQuantity.Should().Be(15);
    }
}
