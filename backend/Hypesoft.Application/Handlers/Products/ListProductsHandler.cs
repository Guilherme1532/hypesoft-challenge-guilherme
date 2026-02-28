using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries.Products;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Handlers.Products;

public class ListProductsHandler : IRequestHandler<ListProductsQuery, PagedResult<ProductDto>>
{
    private readonly IProductRepository _products;

    public ListProductsHandler(IProductRepository products)
    {
        _products = products;
    }

    public async Task<PagedResult<ProductDto>> Handle(ListProductsQuery request, CancellationToken ct)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

        var items = await _products.ListAsync(
            page,
            pageSize,
            request.CategoryId,
            request.Search,
            ct);

        var total = await _products.CountAsync(
            request.CategoryId,
            request.Search,
            ct);

        var dtoItems = items
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.CategoryId,
                p.StockQuantity
            ))
            .ToList();

        return new PagedResult<ProductDto>(
            dtoItems,
            total,
            page,
            pageSize
        );
    }
}