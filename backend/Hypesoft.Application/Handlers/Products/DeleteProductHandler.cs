using Hypesoft.Application.Commands.Products;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Handlers.Products;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _products;

    public DeleteProductHandler(IProductRepository products)
    {
        _products = products;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var existing = await _products.GetByIdAsync(request.Id, ct);

        if (existing is null)
            return false;

        await _products.DeleteAsync(request.Id, ct);

        return true;
    }
}