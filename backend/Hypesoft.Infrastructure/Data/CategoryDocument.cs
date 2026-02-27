using MongoDB.Bson.Serialization.Attributes;

namespace Hypesoft.Infrastructure.Data;

public class CategoryDocument
{
    [BsonId]
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}