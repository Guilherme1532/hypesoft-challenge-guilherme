using AutoMapper;
using FluentAssertions;
using Hypesoft.Application.Commands.Categories;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Handlers.Categories;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Moq;

namespace Hypesoft.Application.Tests.Handlers.Categories;

public class CreateCategoryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateCategory_AndReturnMappedDto()
    {
        // Arrange
        var repo = new Mock<ICategoryRepository>();
        var mapper = new Mock<IMapper>();

        var handler = new CreateCategoryHandler(repo.Object, mapper.Object);

        var command = new CreateCategoryCommand("Eletrônicos");

        Category? createdCategory = null;

        repo.Setup(r => r.CreateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((cat, _) => createdCategory = cat)
            .Returns(Task.CompletedTask);

        mapper.Setup(m => m.Map<CategoryDto>(It.IsAny<Category>()))
            .Returns((Category cat) => new CategoryDto(cat.Id, cat.Name));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        repo.Verify(r => r.CreateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);

        createdCategory.Should().NotBeNull();
        createdCategory!.Name.Should().Be(command.Name);

        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Id.Should().NotBeNullOrWhiteSpace();
    }
}