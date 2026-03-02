using FluentAssertions;
using Hypesoft.Application.Commands.Categories;
using Hypesoft.Application.Handlers.Categories;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Moq;

namespace Hypesoft.Application.Tests.Handlers.Categories;

public class UpdateCategoryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenNotFound()
    {
        // Arrange
        var repo = new Mock<ICategoryRepository>(MockBehavior.Strict);

        var cmd = new UpdateCategoryCommand("missing-id", "New Name");

        repo.Setup(r => r.GetByIdAsync(cmd.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        var handler = new UpdateCategoryHandler(repo.Object);

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        repo.Verify(r => r.GetByIdAsync(cmd.Id, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Never);
        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldUpdateAndReturnTrue_WhenFound()
    {
        // Arrange
        var repo = new Mock<ICategoryRepository>(MockBehavior.Strict);

        var id = "cat-id";
        var existing = new Category("Old");

        var cmd = new UpdateCategoryCommand(id, "New");

        repo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        repo.Setup(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new UpdateCategoryHandler(repo.Object);

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        existing.Name.Should().Be("New");

        repo.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
        repo.VerifyNoOtherCalls();
    }
}