using Hypesoft.Domain.Entities;

namespace Hypesoft.Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(string id, CancellationToken ct = default);

    Task<IReadOnlyList<Product>> ListAsync(
        int page,
        int pageSize,
        string? categoryId = null,
        string? search = null,
        CancellationToken ct = default);

    Task<long> CountAsync(
        string? categoryId = null,
        string? search = null,
        CancellationToken ct = default);

    Task CreateAsync(Product product, CancellationToken ct = default);
    Task UpdateAsync(Product product, CancellationToken ct = default);
    Task DeleteAsync(string id, CancellationToken ct = default);
}