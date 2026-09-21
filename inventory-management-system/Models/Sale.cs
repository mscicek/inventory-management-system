namespace inventory_management_system.Models;

public class Sale
{
    public int Id { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public int WarehouseId { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<SaleItem> Items { get; set; } = new();

    public decimal TotalAmount => Items.Sum(x => x.Total);
}