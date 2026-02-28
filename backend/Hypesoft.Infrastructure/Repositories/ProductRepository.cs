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

    public async Task<IReadOnlyList<Product>> ListAsync(
        int page,
        int pageSize,
        string? categoryId = null,
        string? search = null,
        CancellationToken ct = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var query = _db.Products.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(categoryId))
            query = query.Where(x => x.CategoryId == categoryId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(x => x.Name.Contains(s));
        }

        var skip = (page - 1) * pageSize;

        var docs = await query
            .OrderBy(x => x.Name)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);

        return docs
            .Select(d => Product.Rehydrate(d.Id, d.Name, d.Description, d.Price, d.CategoryId, d.StockQuantity))
            .ToList();
    }

    public async Task<long> CountAsync(
        string? categoryId = null,
        string? search = null,
        CancellationToken ct = default)
    {
        var query = _db.Products.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(categoryId))
            query = query.Where(x => x.CategoryId == categoryId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(x => x.Name.Contains(s));
        }

        return await query.LongCountAsync(ct);
    }

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        var doc = await _db.Products.FirstOrDefaultAsync(x => x.Id == product.Id, ct);
        if (doc is null) return;

        doc.Name = product.Name;
        doc.Description = product.Description;
        doc.Price = product.Price;
        doc.CategoryId = product.CategoryId;
        doc.StockQuantity = product.StockQuantity;

        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default)
    {
        var doc = await _db.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (doc is null) return;

        _db.Products.Remove(doc);
        await _db.SaveChangesAsync(ct);
    }
}