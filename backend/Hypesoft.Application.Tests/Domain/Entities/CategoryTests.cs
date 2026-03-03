using FluentAssertions;
using Hypesoft.Domain.Entities;

namespace Hypesoft.Application.Tests.Domain.Entities;

public class CategoryTests
{
    [Fact]
    public void Constructor_ShouldSetTrimmedName()
    {
        var category = new Category("  Eletronicos  ");

        category.Name.Should().Be("Eletronicos");
        category.Id.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsEmpty()
    {
        var act = () => new Category("   ");

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Category name is required.*");
    }

    [Fact]
    public void SetName_ShouldUpdateTrimmedName()
    {
        var category = new Category("Inicial");

        category.SetName("  Atualizada ");

        category.Name.Should().Be("Atualizada");
    }

    [Fact]
    public void SetName_ShouldThrow_WhenNameIsNull()
    {
        var category = new Category("Inicial");

        var act = () => category.SetName(null!);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Category name is required.*");
    }

    [Fact]
    public void Rehydrate_ShouldKeepProvidedIdAndName()
    {
        var category = Category.Rehydrate("cat-123", "Papelaria");

        category.Id.Should().Be("cat-123");
        category.Name.Should().Be("Papelaria");
    }
}
