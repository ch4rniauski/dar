namespace laba2.Models;

public sealed class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }

    public Product(
        int id,
        string name,
        string description,
        decimal price)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
    }
}
