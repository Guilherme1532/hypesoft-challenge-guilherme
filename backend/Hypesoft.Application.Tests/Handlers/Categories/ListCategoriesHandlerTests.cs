using AutoMapper;
using FluentAssertions;
using Hypesoft.Application.Handlers.Categories;
using Hypesoft.Application.Mapping;
using Hypesoft.Application.Queries.Categories;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Moq;

namespace Hypesoft.Application.Tests.Handlers.Categories;

public class ListCategoriesHandlerTests
{
    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CategoryProfile>());
        config.AssertConfigurationIsValid();
        return config.CreateMapper();
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedList()
    {
        // arrange
        var repo = new Mock<ICategoryRepository>();
        var mapper = CreateMapper();
        var handler = new ListCategoriesHandler(repo.Object, mapper);

        var data = new List<Category>
        {
            new Category("A"),
            new Category("B")
        };

        repo.Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(data);

        // act
        var result = await handler.Handle(new ListCategoriesQuery(), CancellationToken.None);

        // assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Select(x => x.Name).Should().Contain(new[] { "A", "B" });

        repo.Verify(r => r.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}