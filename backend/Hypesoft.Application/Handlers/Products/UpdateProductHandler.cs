using Hypesoft.Application.Commands.Products;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Handlers.Products;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _products;

    public UpdateProductHandler(IProductRepository products)
    {
        _products = products;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var product = await _products.GetByIdAsync(request.Id, ct);

        if (product is null)
            return false;

        product.SetName(request.Name);
        product.SetDescription(request.Description);
        product.SetPrice(request.Price);
        product.SetCategory(request.CategoryId);
        product.SetStock(request.StockQuantity);

        await _products.UpdateAsync(product, ct);

        return true;
    }
}