using MediatR;
using Hypesoft.Application.Commands;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;

namespace Hypesoft.Application.Handlers;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categories;

    public CreateCategoryHandler(ICategoryRepository categories)
    {
        _categories = categories;
    }

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        var category = new Category(request.Name);

        await _categories.CreateAsync(category, ct);

        return new CategoryDto(category.Id, category.Name);
    }
}