using FluentAssertions;
using Hypesoft.Application.Commands.Categories;
using Hypesoft.Application.Validators.Categories;

namespace Hypesoft.Application.Tests.Validators.Categories;

public class CreateCategoryValidatorTests
{
    private readonly CreateCategoryValidator _validator = new();

    [Fact]
    public void Should_fail_when_name_is_empty()
    {
        var command = new CreateCategoryCommand("");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_fail_when_name_exceeds_max_length()
    {
        var longName = new string('A', 121);

        var command = new CreateCategoryCommand(longName);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_pass_when_name_is_valid()
    {
        var command = new CreateCategoryCommand("Eletrônicos");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}