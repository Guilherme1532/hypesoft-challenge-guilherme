using FluentAssertions;
using Hypesoft.Application.Commands.Products;
using Hypesoft.Application.Handlers.Products;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Moq;

namespace Hypesoft.Application.Tests.Handlers.Products;

public class UpdateProductStockHandlerTests
{
    [Fact]
    public async Task Handle_WhenProductDoesNotExist_ShouldReturnFalse_AndNotCallUpdate()
    {
        // Arrange
        var repo = new Mock<IProductRepository>();

        var cmd = new UpdateProductStockCommand(
            Id: "p1",
            StockQuantity: 10
        );

        repo.Setup(r => r.GetByIdAsync(cmd.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var handler = new UpdateProductStockHandler(repo.Object);

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        repo.Verify(r => r.GetByIdAsync(cmd.Id, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenProductExists_ShouldSetStock_CallUpdate_AndReturnTrue()
    {
        // Arrange
        var repo = new Mock<IProductRepository>();

        var existing = new Product(
            name: "Old name",
            description: "Old desc",
            price: 10m,
            categoryId: "cat1",
            stockQuantity: 1
        );

        var cmd = new UpdateProductStockCommand(
            Id: "p1",
            StockQuantity: 10
        );

        repo.Setup(r => r.GetByIdAsync(cmd.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        repo.Setup(r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new UpdateProductStockHandler(repo.Object);

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        repo.Verify(r => r.GetByIdAsync(cmd.Id, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(
            r => r.UpdateAsync(
                It.Is<Product>(p => p.StockQuantity == cmd.StockQuantity),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}