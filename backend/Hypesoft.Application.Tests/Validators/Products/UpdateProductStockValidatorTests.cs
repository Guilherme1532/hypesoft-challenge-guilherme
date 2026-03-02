using FluentAssertions;
using Hypesoft.Application.Commands.Products;
using Hypesoft.Application.Validators.Products;

namespace Hypesoft.Application.Tests.Validators.Products;

public class UpdateProductStockValidatorTests
{
    private readonly UpdateProductStockValidator _validator = new();

    [Fact]
    public void Should_fail_when_id_is_empty()
    {
        var command = new UpdateProductStockCommand(
            Id: "",
            StockQuantity: 10
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id");
    }

    [Fact]
    public void Should_fail_when_stock_is_negative()
    {
        var command = new UpdateProductStockCommand(
            Id: "abc123",
            StockQuantity: -1
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "StockQuantity");
    }

    [Fact]
    public void Should_pass_when_command_is_valid()
    {
        var command = new UpdateProductStockCommand(
            Id: "abc123",
            StockQuantity: 0
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}