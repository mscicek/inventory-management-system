namespace inventory_management_system.Models;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int MinimumStock { get; set; }

    public int CategoryId { get; set; }

    public decimal CalculateDiscountedPrice(decimal discountPercentage)
    {
        var discountAmount = Price * discountPercentage / 100;

        return Price - discountAmount;
    }
}