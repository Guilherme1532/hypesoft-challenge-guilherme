using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries.Products;
using Hypesoft.Domain.Repositories;
using MediatR;
using AutoMapper;

namespace Hypesoft.Application.Handlers.Products;

public class ListLowStockProductsHandler : IRequestHandler<ListLowStockProductsQuery, IReadOnlyList<ProductDto>>
{
    private readonly IProductRepository _products;

    private readonly IMapper _mapper;

    public ListLowStockProductsHandler(IProductRepository products, IMapper mapper)
    {
        _products = products;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProductDto>> Handle(ListLowStockProductsQuery request, CancellationToken ct)
    {
        var items = await _products.ListLowStockAsync(request.Threshold, request.Limit, ct);

        return _mapper.Map<List<ProductDto>>(items);
    }
}