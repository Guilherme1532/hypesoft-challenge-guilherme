using FluentAssertions;
using Hypesoft.Application.Commands.Categories;
using Hypesoft.Application.Validators.Categories;
using Xunit;

namespace Hypesoft.Application.Tests.Validators.Categories;

public class DeleteCategoryValidatorTests
{
    private readonly DeleteCategoryValidator _validator = new();

    [Fact]
    public void Should_fail_when_id_is_empty()
    {
        var command = new DeleteCategoryCommand("");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id");
    }

    [Fact]
    public void Should_pass_when_id_is_provided()
    {
        var command = new DeleteCategoryCommand("abc123");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}