using Microsoft.EntityFrameworkCore;

namespace Hypesoft.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(HypesoftDbContext db, CancellationToken ct = default)
    {
        var hasCategories = await db.Categories.AnyAsync(ct);
        var hasProducts = await db.Products.AnyAsync(ct);

        if (hasCategories || hasProducts)
        {
            return;
        }

        var categories = new[]
        {
            new CategoryDocument { Id = "cat-electronics", Name = "Eletronicos" },
            new CategoryDocument { Id = "cat-office", Name = "Escritorio" },
            new CategoryDocument { Id = "cat-home", Name = "Casa" },
            new CategoryDocument { Id = "cat-accessories", Name = "Acessorios" }
        };

        var products = new[]
        {
            new ProductDocument
            {
                Id = "prod-mouse-gamer",
                Name = "Mouse Gamer",
                Description = "Mouse RGB com 7 botoes programaveis",
                Price = 189.90m,
                CategoryId = "cat-electronics",
                StockQuantity = 18
            },
            new ProductDocument
            {
                Id = "prod-keyboard-mech",
                Name = "Teclado Mecanico",
                Description = "Teclado mecanico switch brown ABNT2",
                Price = 329.00m,
                CategoryId = "cat-electronics",
                StockQuantity = 7
            },
            new ProductDocument
            {
                Id = "prod-monitor-24",
                Name = "Monitor 24\"",
                Description = "Monitor IPS Full HD 75Hz",
                Price = 899.00m,
                CategoryId = "cat-electronics",
                StockQuantity = 5
            },
            new ProductDocument
            {
                Id = "prod-cadeira-office",
                Name = "Cadeira Office",
                Description = "Cadeira ergonomica para escritorio",
                Price = 749.90m,
                CategoryId = "cat-office",
                StockQuantity = 4
            },
            new ProductDocument
            {
                Id = "prod-escrivaninha",
                Name = "Escrivaninha",
                Description = "Mesa de trabalho 120cm",
                Price = 459.00m,
                CategoryId = "cat-office",
                StockQuantity = 12
            },
            new ProductDocument
            {
                Id = "prod-luminaria",
                Name = "Luminaria de Mesa",
                Description = "Luminaria LED com ajuste de intensidade",
                Price = 119.90m,
                CategoryId = "cat-home",
                StockQuantity = 26
            },
            new ProductDocument
            {
                Id = "prod-suporte-notebook",
                Name = "Suporte para Notebook",
                Description = "Suporte ajustavel em aluminio",
                Price = 89.90m,
                CategoryId = "cat-accessories",
                StockQuantity = 32
            },
            new ProductDocument
            {
                Id = "prod-headset",
                Name = "Headset USB",
                Description = "Headset com microfone e cancelamento de ruido",
                Price = 249.00m,
                CategoryId = "cat-accessories",
                StockQuantity = 9
            }
        };

        await db.Categories.AddRangeAsync(categories, ct);
        await db.Products.AddRangeAsync(products, ct);
        await db.SaveChangesAsync(ct);
    }
}
