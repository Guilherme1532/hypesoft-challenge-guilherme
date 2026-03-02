using FluentAssertions;
using Hypesoft.Application.Commands.Products;
using Hypesoft.Application.Validators.Products;

namespace Hypesoft.Application.Tests.Validators.Products;

public class DeleteProductValidatorTests
{
    private readonly DeleteProductValidator _validator = new();

    [Fact]
    public void Should_fail_when_id_is_empty()
    {
        var cmd = new DeleteProductCommand("");

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id");
    }

    [Fact]
    public void Should_pass_when_id_is_provided()
    {
        var cmd = new DeleteProductCommand("2f9fdadf689749eb83117db3aa227cd9");

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeTrue();
    }
}