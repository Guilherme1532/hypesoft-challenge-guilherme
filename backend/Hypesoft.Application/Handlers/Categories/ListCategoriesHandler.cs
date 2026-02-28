using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries.Categories;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Handlers.Categories;

public class ListCategoriesHandler : IRequestHandler<ListCategoriesQuery, IReadOnlyList<CategoryDto>>
{
    private readonly ICategoryRepository _repository;

    public ListCategoriesHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<CategoryDto>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repository.ListAsync(cancellationToken);
        return categories.Select(c => new CategoryDto
        (
            c.Id,
            c.Name
        )).ToList();
    }
}
