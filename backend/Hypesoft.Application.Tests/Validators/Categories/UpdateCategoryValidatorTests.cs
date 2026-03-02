using FluentAssertions;
using Hypesoft.Application.Commands.Categories;
using Hypesoft.Application.Validators.Categories;

namespace Hypesoft.Application.Tests.Validators.Categories;

public class UpdateCategoryValidatorTests
{
    private readonly UpdateCategoryValidator _validator = new();

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

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_fail_when_name_exceeds_max_length()
    {
        var longName = new string('A', 101);

        var command = CreateValidCommand() with { Name = longName };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_pass_when_command_is_valid()
    {
        var command = CreateValidCommand();

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    private static UpdateCategoryCommand CreateValidCommand()
    {
        return new UpdateCategoryCommand(
            Id: "abc123",
            Name: "Categoria válida"
        );
    }
}