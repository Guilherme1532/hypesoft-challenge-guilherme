using AutoMapper;
using FluentAssertions;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Handlers.Products;
using Hypesoft.Application.Queries.Products;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Moq;

namespace Hypesoft.Application.Tests.Handlers.Products;

public class GetProductByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var repoMock = new Mock<IProductRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock
            .Setup(r => r.GetByIdAsync("id-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var handler = new GetProductByIdHandler(repoMock.Object, mapperMock.Object);
        var query = new GetProductByIdQuery("id-1");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();

        mapperMock.Verify(m => m.Map<ProductDto>(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedDto_WhenProductExists()
    {
        // Arrange
        var repoMock = new Mock<IProductRepository>();
        var mapperMock = new Mock<IMapper>();

        var product = new Product(
            name: "Produto",
            description: "Desc",
            price: 10,
            categoryId: "cat",
            stockQuantity: 5
        );

        var expectedDto = new ProductDto(
            Id: "any",
            Name: product.Name,
            Description: product.Description,
            Price: product.Price,
            CategoryId: product.CategoryId,
            StockQuantity: product.StockQuantity
        );

        repoMock
            .Setup(r => r.GetByIdAsync("id-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        mapperMock
            .Setup(m => m.Map<ProductDto>(product))
            .Returns(expectedDto);

        var handler = new GetProductByIdHandler(repoMock.Object, mapperMock.Object);
        var query = new GetProductByIdQuery("id-1");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedDto);

        mapperMock.Verify(m => m.Map<ProductDto>(product), Times.Once);
    }
}