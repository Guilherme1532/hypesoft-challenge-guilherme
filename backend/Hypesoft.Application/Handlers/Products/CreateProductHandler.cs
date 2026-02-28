using MediatR;
using Hypesoft.Application.Commands.Products;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;

namespace Hypesoft.Application.Handlers.Products;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _products;

    public CreateProductHandler(IProductRepository products)
    {
        _products = products;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = new Product(
            request.Name,
            request.Description,
            request.Price,
            request.CategoryId,
            request.StockQuantity
        );

        await _products.CreateAsync(product, ct);

        return new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.CategoryId,
            product.StockQuantity
        );
    }
}