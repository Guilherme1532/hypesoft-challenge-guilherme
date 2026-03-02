using FluentValidation.TestHelper;
using FluentAssertions;
using Hypesoft.Application.Commands.Products;
using Hypesoft.Application.Validators.Products;

namespace Hypesoft.Application.Tests.Validators.Products;

public class CreateProductValidatorTests
{
    private readonly CreateProductValidator _validator = new();

    [Fact]
    public void Should_have_error_when_name_is_empty()
    {
        var command = new CreateProductCommand(
            Name: "",
            Description: "desc",
            Price: 10,
            CategoryId: "cat",
            StockQuantity: 5
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_have_error_when_price_is_negative()
    {
        var command = new CreateProductCommand(
            Name: "Produto",
            Description: "desc",
            Price: -1,
            CategoryId: "cat",
            StockQuantity: 5
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Should_pass_when_command_is_valid()
    {
        var command = new CreateProductCommand(
            Name: "Produto",
            Description: "desc",
            Price: 10,
            CategoryId: "cat",
            StockQuantity: 5
        );

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeTrue();
    }
}