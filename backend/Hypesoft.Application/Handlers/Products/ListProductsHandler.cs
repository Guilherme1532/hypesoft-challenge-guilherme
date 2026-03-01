using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries.Products;
using Hypesoft.Domain.Repositories;
using MediatR;
using AutoMapper;

namespace Hypesoft.Application.Handlers.Products;

public class ListProductsHandler : IRequestHandler<ListProductsQuery, PagedResult<ProductDto>>
{
    private readonly IProductRepository _products;

    private readonly IMapper _mapper;

    public ListProductsHandler(IProductRepository products, IMapper mapper)
    {
        _products = products;
        _mapper = mapper;
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

        var dtoItems = _mapper.Map<List<ProductDto>>(items);

        return new PagedResult<ProductDto>(
            dtoItems,
            total,
            page,
            pageSize
        );
    }
}