using Hypesoft.Application.DTOs;
using MediatR;

namespace Hypesoft.Application.Queries.Products;

public record ListProductsQuery(
    int Page = 1,
    int PageSize = 10,
    string? CategoryId = null,
    string? Search = null
) : IRequest<PagedResult<ProductDto>>;