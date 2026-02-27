namespace Hypesoft.Domain.Entities;

public class Product
{
    public string Id { get; private set; } = Guid.NewGuid().ToString("N");

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    public string CategoryId { get; private set; } = string.Empty;

    public int StockQuantity { get; private set; }

    private Product()
    {
        Name = string.Empty;
        Description = string.Empty;
        CategoryId = string.Empty;
    }

    public Product(string name, string description, decimal price, string categoryId, int stockQuantity)
    {
        SetName(name);
        SetDescription(description);
        SetPrice(price);
        SetCategory(categoryId);
        SetStock(stockQuantity);
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required.", nameof(name));

        Name = name.Trim();
    }

    public void SetDescription(string description)
    {
        Description = (description ?? string.Empty).Trim();
    }

    public void SetPrice(decimal price)
    {
        if (price < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(price));

        Price = price;
    }

    public void SetCategory(string categoryId)
    {
        if (string.IsNullOrWhiteSpace(categoryId))
            throw new ArgumentException("Category is required.", nameof(categoryId));

        CategoryId = categoryId.Trim();
    }

    public void SetStock(int stockQuantity)
    {
        if (stockQuantity < 0)
            throw new ArgumentException("Stock cannot be negative.", nameof(stockQuantity));

        StockQuantity = stockQuantity;
    }

    public bool IsLowStock(int threshold = 10) => StockQuantity < threshold;

    public static Product Rehydrate(string id, string name, string description, decimal price, string categoryId, int stockQuantity)
    {
        var p = new Product(name, description, price, categoryId, stockQuantity);
        p.Id = id;
        return p;
    }
}

