using MediatR;
using Hypesoft.Application.DTOs;

namespace Hypesoft.Application.Commands.Categories;

public record CreateCategoryCommand(
    string Name
) : IRequest<CategoryDto>;