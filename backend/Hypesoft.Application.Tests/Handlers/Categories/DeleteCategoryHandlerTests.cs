using FluentAssertions;
using Hypesoft.Application.Commands.Categories;
using Hypesoft.Application.Handlers.Categories;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Moq;

namespace Hypesoft.Application.Tests.Handlers.Categories;

public class DeleteCategoryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_False_When_Category_Not_Found()
    {
        // Arrange
        var repo = new Mock<ICategoryRepository>();

        var command = new DeleteCategoryCommand("missing-id");

        repo.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        var handler = new DeleteCategoryHandler(repo.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        repo.Verify(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Delete_And_Return_True_When_Category_Exists()
    {
        // Arrange
        var repo = new Mock<ICategoryRepository>();

        var command = new DeleteCategoryCommand("cat-1");
        var existing = new Category("Eletrônicos");

        repo.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        repo.Setup(r => r.DeleteAsync(command.Id, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new DeleteCategoryHandler(repo.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        repo.Verify(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.DeleteAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}
