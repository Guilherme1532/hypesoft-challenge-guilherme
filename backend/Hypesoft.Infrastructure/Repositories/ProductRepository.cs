using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Hypesoft.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hypesoft.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly HypesoftDbContext _db;

    public ProductRepository(HypesoftDbContext db)
    {
        _db = db;
    }

    public async Task CreateAsync(Product product, CancellationToken ct = default)
    {
        var doc = new ProductDocument
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            StockQuantity = product.StockQuantity
        };

        await _db.Products.AddAsync(doc, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<Product?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var doc = await _db.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        return doc is null
            ? null
            : Product.Rehydrate(doc.Id, doc.Name, doc.Description, doc.Price, doc.CategoryId, doc.StockQuantity);
    }

    // Vamos implementar esses depois (list/pagination/search, update, delete)
    public Task<IReadOnlyList<Product>> ListAsync(int page, int pageSize, string? categoryId = null, string? search = null, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<long> CountAsync(string? categoryId = null, string? search = null, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task UpdateAsync(Product product, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task DeleteAsync(string id, CancellationToken ct = default)
        => throw new NotImplementedException();
}