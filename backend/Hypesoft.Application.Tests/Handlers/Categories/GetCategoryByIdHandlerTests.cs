using AutoMapper;
using FluentAssertions;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Handlers.Categories;
using Hypesoft.Application.Queries.Categories;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Moq;

namespace Hypesoft.Application.Tests.Handlers.Categories;

public class GetCategoryByIdHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Null_When_Category_Not_Found()
    {
        // Arrange
        var repo = new Mock<ICategoryRepository>();
        var mapper = new Mock<IMapper>();

        var query = new GetCategoryByIdQuery("missing-id");

        repo.Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        var handler = new GetCategoryByIdHandler(repo.Object, mapper.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();

        repo.Verify(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()), Times.Once);
        mapper.Verify(m => m.Map<CategoryDto>(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Return_Mapped_Dto_When_Category_Exists()
    {
        // Arrange
        var repo = new Mock<ICategoryRepository>();
        var mapper = new Mock<IMapper>();

        var query = new GetCategoryByIdQuery("cat-1");
        var category = new Category("Eletrônicos");
        var expectedDto = new CategoryDto("cat-1", "Eletrônicos");

        repo.Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        mapper.Setup(m => m.Map<CategoryDto>(category))
              .Returns(expectedDto);

        var handler = new GetCategoryByIdHandler(repo.Object, mapper.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedDto);

        repo.Verify(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()), Times.Once);
        mapper.Verify(m => m.Map<CategoryDto>(category), Times.Once);
    }
}