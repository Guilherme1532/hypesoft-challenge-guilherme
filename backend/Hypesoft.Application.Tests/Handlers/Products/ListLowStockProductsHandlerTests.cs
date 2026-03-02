using AutoMapper;
using FluentAssertions;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Handlers.Products;
using Hypesoft.Application.Queries.Products;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Moq;

namespace Hypesoft.Application.Tests.Handlers.Products;

public class ListLowStockProductsHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCallRepoAndReturnMappedList()
    {
        // Arrange
        var repo = new Mock<IProductRepository>();
        var mapper = new Mock<IMapper>();

        var items = new List<Product>
        {
            new Product("A", "D", 10, "cat1", 2),
            new Product("B", "D", 20, "cat1", 3)
        };

        var mapped = new List<ProductDto>
        {
            new ProductDto("1", "A", "D", 10, "cat1", 2),
            new ProductDto("2", "B", "D", 20, "cat1", 3),
        };

        repo.Setup(r => r.ListLowStockAsync(10, 50, It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);

        mapper.Setup(m => m.Map<List<ProductDto>>(items))
              .Returns(mapped);

        var handler = new ListLowStockProductsHandler(repo.Object, mapper.Object);

        // Act
        var result = await handler.Handle(new ListLowStockProductsQuery(10, 50), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(mapped);

        repo.Verify(r => r.ListLowStockAsync(10, 50, It.IsAny<CancellationToken>()), Times.Once);
        mapper.Verify(m => m.Map<List<ProductDto>>(items), Times.Once);
    }
}