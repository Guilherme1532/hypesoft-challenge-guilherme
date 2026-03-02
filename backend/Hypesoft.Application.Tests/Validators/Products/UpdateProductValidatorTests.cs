using FluentAssertions;
using Hypesoft.Application.Commands.Products;
using Hypesoft.Application.Validators.Products;

namespace Hypesoft.Application.Tests.Validators.Products;

public class UpdateProductValidatorTests
{
    private readonly UpdateProductValidator _validator = new();

    [Fact]
    public void Should_fail_when_id_is_empty()
    {
        var command = CreateValidCommand() with { Id = "" };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id");
    }

    [Fact]
    public void Should_fail_when_name_is_empty()
    {
        var command = CreateValidCommand() with { Name = "" };

        var result = _validator.Validate(command);

        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_fail_when_name_exceeds_max_length()
    {
        var longName = new string('A', 121);

        var command = CreateValidCommand() with { Name = longName };

        var result = _validator.Validate(command);

        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_fail_when_price_is_negative()
    {
        var command = CreateValidCommand() with { Price = -1 };

        var result = _validator.Validate(command);

        result.Errors.Should().Contain(e => e.PropertyName == "Price");
    }

    [Fact]
    public void Should_fail_when_category_is_empty()
    {
        var command = CreateValidCommand() with { CategoryId = "" };

        var result = _validator.Validate(command);

        result.Errors.Should().Contain(e => e.PropertyName == "CategoryId");
    }

    [Fact]
    public void Should_fail_when_stock_is_negative()
    {
        var command = CreateValidCommand() with { StockQuantity = -1 };

        var result = _validator.Validate(command);

        result.Errors.Should().Contain(e => e.PropertyName == "StockQuantity");
    }

    [Fact]
    public void Should_pass_when_command_is_valid()
    {
        var command = CreateValidCommand();

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    private static UpdateProductCommand CreateValidCommand()
    {
        return new UpdateProductCommand(
            Id: "abc123",
            Name: "Produto",
            Description: "Descrição válida",
            Price: 10,
            CategoryId: "cat123",
            StockQuantity: 5
        );
    }
}