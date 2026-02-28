using Hypesoft.Application.DTOs;
using MediatR;

namespace Hypesoft.Application.Queries.Categories;

public record ListCategoriesQuery : IRequest<IReadOnlyList<CategoryDto>>;