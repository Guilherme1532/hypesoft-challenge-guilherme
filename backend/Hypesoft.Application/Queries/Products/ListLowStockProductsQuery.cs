using Hypesoft.Application.DTOs;
using MediatR;

namespace Hypesoft.Application.Queries.Products;

public record ListLowStockProductsQuery(int Threshold = 10, int Limit = 10) : IRequest<IReadOnlyList<ProductDto>>;