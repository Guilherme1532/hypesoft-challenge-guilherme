using AutoMapper;
using FluentAssertions;
using Hypesoft.Application.Commands.Products;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Handlers.Products;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Moq;

namespace Hypesoft.Application.Tests.Handlers.Products;

public class CreateProductHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateProduct_AndReturnMappedDto()
    {
        
        var repoMock = new Mock<IProductRepository>();
        var mapperMock = new Mock<IMapper>();

        var handler = new CreateProductHandler(repoMock.Object, mapperMock.Object);

        var cmd = new CreateProductCommand(
            Name: "Mouse Gamer",
            Description: "RGB",
            Price: 199.90m,
            CategoryId: "cat-1",
            StockQuantity: 10
        );

        var expectedDto = new ProductDto(
            Id: "any",
            Name: cmd.Name,
            Description: cmd.Description,
            Price: cmd.Price,
            CategoryId: cmd.CategoryId,
            StockQuantity: cmd.StockQuantity
        );

        
        mapperMock
            .Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
            .Returns(expectedDto);

        
        var result = await handler.Handle(cmd, CancellationToken.None);

        
        result.Should().Be(expectedDto);

        repoMock.Verify(r => r.CreateAsync(
            It.Is<Product>(p =>
                p.Name == cmd.Name &&
                p.Description == cmd.Description &&
                p.Price == cmd.Price &&
                p.CategoryId == cmd.CategoryId &&
                p.StockQuantity == cmd.StockQuantity
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);

        mapperMock.Verify(m => m.Map<ProductDto>(
            It.IsAny<Product>()
        ), Times.Once);
    }
}