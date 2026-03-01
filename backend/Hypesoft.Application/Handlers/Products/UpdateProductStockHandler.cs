using Hypesoft.Application.Commands.Products;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Handlers.Products;

public class UpdateProductStockHandler : IRequestHandler<UpdateProductStockCommand, bool>
{
    private readonly IProductRepository _products;

    public UpdateProductStockHandler(IProductRepository products)
    {
        _products = products;
    }

    public async Task<bool> Handle(UpdateProductStockCommand request, CancellationToken ct)
    {
        var product = await _products.GetByIdAsync(request.Id, ct);
        if (product is null)
            return false;

        product.SetStock(request.StockQuantity);

        await _products.UpdateAsync(product, ct);

        return true;
    }
}