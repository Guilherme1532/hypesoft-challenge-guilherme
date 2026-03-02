using FluentAssertions;
using Hypesoft.Application.Commands.Products;
using Hypesoft.Application.Handlers.Products;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Moq;

namespace Hypesoft.Application.Tests.Handlers.Products;

public class DeleteProductHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenProductDoesNotExist()
    {
        // Arrange
        var repoMock = new Mock<IProductRepository>();

        repoMock
            .Setup(r => r.GetByIdAsync("id-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var handler = new DeleteProductHandler(repoMock.Object);
        var command = new DeleteProductCommand("id-1");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        repoMock.Verify(r => r.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldDeleteProduct_AndReturnTrue_WhenProductExists()
    {
        // Arrange
        var repoMock = new Mock<IProductRepository>();

        var existingProduct = new Product(
            name: "Produto",
            description: "Desc",
            price: 10,
            categoryId: "cat",
            stockQuantity: 5
        );

        repoMock
            .Setup(r => r.GetByIdAsync("id-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        var handler = new DeleteProductHandler(repoMock.Object);
        var command = new DeleteProductCommand("id-1");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        repoMock.Verify(r => r.DeleteAsync("id-1", It.IsAny<CancellationToken>()), Times.Once);
    }
}