using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries.Products;
using Hypesoft.Domain.Repositories;
using MediatR;
using AutoMapper;

namespace Hypesoft.Application.Handlers.Products;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _products;

    private readonly IMapper _mapper;

    public GetProductByIdHandler(IProductRepository products, IMapper mapper)
    {
        _products = products;
        _mapper = mapper;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await _products.GetByIdAsync(request.Id, ct);

        if (product is null)
            return null;

        return _mapper.Map<ProductDto>(product);
    }
}