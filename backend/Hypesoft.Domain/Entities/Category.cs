namespace Hypesoft.Domain.Entities;

public class Category
{
    public string Id { get; private set; } = Guid.NewGuid().ToString("N");
    public string Name { get; private set; } = string.Empty;

    private Category() { Name = string.Empty; }

    public Category(string name)
    {
        SetName(name);
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.", nameof(name));

        Name = name.Trim();
    }

    public static Category Rehydrate(string id, string name)
    {
        var category = new Category(name);
        category.Id = id;
        return category;
    }
}