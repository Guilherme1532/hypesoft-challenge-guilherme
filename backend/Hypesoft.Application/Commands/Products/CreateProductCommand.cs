using MediatR;
using Hypesoft.Application.DTOs;

namespace Hypesoft.Application.Commands.Products;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    string CategoryId,
    int StockQuantity
) : IRequest<ProductDto>;