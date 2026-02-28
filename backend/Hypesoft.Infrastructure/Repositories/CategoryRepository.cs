using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Hypesoft.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hypesoft.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly HypesoftDbContext _db;

    public CategoryRepository(HypesoftDbContext db)
    {
        _db = db;
    }

    public async Task CreateAsync(Category category, CancellationToken ct = default)
    {
        var doc = new CategoryDocument
        {
            Id = category.Id,
            Name = category.Name
        };

        await _db.Categories.AddAsync(doc, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<Category?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var doc = await _db.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        return doc is null ? null : Category.Rehydrate(doc.Id, doc.Name);
    }

    public async Task<IReadOnlyList<Category>> ListAsync(CancellationToken ct = default)
    {
        var docs = await _db.Categories
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(ct);

        return docs
            .Select(d => Category.Rehydrate(d.Id, d.Name))
            .ToList();
    }

    public async Task UpdateAsync(Category category, CancellationToken ct = default)
    {
        var doc = await _db.Categories
            .FirstOrDefaultAsync(x => x.Id == category.Id, ct);

        if (doc is null)
            return; // ou lançar NotFound

        doc.Name = category.Name;

        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default)
    {
        var doc = await _db.Categories
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (doc is null)
            return;

        _db.Categories.Remove(doc);
        await _db.SaveChangesAsync(ct);
    }
}