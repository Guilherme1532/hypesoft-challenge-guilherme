using MongoDB.Bson.Serialization.Attributes;

namespace Hypesoft.Infrastructure.Data;

public class ProductDocument
{
    [BsonId]
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public string CategoryId { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
}