using AutoMapper;
using FluentAssertions;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Handlers.Products;
using Hypesoft.Application.Queries.Products;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Moq;

namespace Hypesoft.Application.Tests.Handlers.Products;

public class ListProductsHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCallRepoAndReturnPagedResultWithMappedItems()
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

        const int page = 2;
        const int pageSize = 25;
        const string categoryId = "cat1";
        const string search = "abc";
        const int total = 123;

        repo.Setup(r => r.ListAsync(page, pageSize, categoryId, search, It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);

        repo.Setup(r => r.CountAsync(categoryId, search, It.IsAny<CancellationToken>()))
            .ReturnsAsync(total);

        mapper.Setup(m => m.Map<List<ProductDto>>(items))
              .Returns(mapped);

        var handler = new ListProductsHandler(repo.Object, mapper.Object);

        // Act
        var result = await handler.Handle(new ListProductsQuery(page, pageSize, categoryId, search), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEquivalentTo(mapped);
        result.Total.Should().Be(total);
        result.Page.Should().Be(page);
        result.PageSize.Should().Be(pageSize);

        repo.Verify(r => r.ListAsync(page, pageSize, categoryId, search, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.CountAsync(categoryId, search, It.IsAny<CancellationToken>()), Times.Once);
        mapper.Verify(m => m.Map<List<ProductDto>>(items), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenPageOrPageSizeIsInvalid_ShouldDefaultToOneAndTen()
    {
        // Arrange
        var repo = new Mock<IProductRepository>();
        var mapper = new Mock<IMapper>();

        var items = new List<Product>
        {
            new Product("A", "D", 10, "cat1", 2)
        };

        var mapped = new List<ProductDto>
        {
            new ProductDto("1", "A", "D", 10, "cat1", 2),
        };

        const int requestedPage = 0;
        const int requestedPageSize = 0;

        const int normalizedPage = 1;
        const int normalizedPageSize = 10;

        string? categoryId = null;
        string? search = null;
        const int total = 1;

        repo.Setup(r => r.ListAsync(normalizedPage, normalizedPageSize, categoryId, search, It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);

        repo.Setup(r => r.CountAsync(categoryId, search, It.IsAny<CancellationToken>()))
            .ReturnsAsync(total);

        mapper.Setup(m => m.Map<List<ProductDto>>(items))
              .Returns(mapped);

        var handler = new ListProductsHandler(repo.Object, mapper.Object);

        // Act
        var result = await handler.Handle(new ListProductsQuery(requestedPage, requestedPageSize, categoryId, search), CancellationToken.None);

        // Assert
        result.Items.Should().BeEquivalentTo(mapped);
        result.Total.Should().Be(total);
        result.Page.Should().Be(normalizedPage);
        result.PageSize.Should().Be(normalizedPageSize);

        repo.Verify(r => r.ListAsync(normalizedPage, normalizedPageSize, categoryId, search, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.CountAsync(categoryId, search, It.IsAny<CancellationToken>()), Times.Once);
        mapper.Verify(m => m.Map<List<ProductDto>>(items), Times.Once);
    }
}