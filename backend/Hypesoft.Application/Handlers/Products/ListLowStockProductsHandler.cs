using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries.Products;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Handlers.Products;

public class ListLowStockProductsHandler : IRequestHandler<ListLowStockProductsQuery, IReadOnlyList<ProductDto>>
{
    private readonly IProductRepository _products;

    public ListLowStockProductsHandler(IProductRepository products)
    {
        _products = products;
    }

    public async Task<IReadOnlyList<ProductDto>> Handle(ListLowStockProductsQuery request, CancellationToken ct)
    {
        var items = await _products.ListLowStockAsync(request.Threshold, request.Limit, ct);

        return items.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.CategoryId,
            p.StockQuantity
        )).ToList();
    }
}