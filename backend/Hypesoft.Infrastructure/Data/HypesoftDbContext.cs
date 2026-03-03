using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Hypesoft.Infrastructure.Data;

public class HypesoftDbContext : DbContext
{
    public HypesoftDbContext(DbContextOptions<HypesoftDbContext> options) : base(options)
    {
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }

    public DbSet<ProductDocument> Products => Set<ProductDocument>();
    public DbSet<CategoryDocument> Categories => Set<CategoryDocument>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapeia para collections do Mongo
        modelBuilder.Entity<ProductDocument>().ToCollection("products");
        modelBuilder.Entity<CategoryDocument>().ToCollection("categories");

        // (opcional agora) índices — a gente faz depois
    }
}
