namespace inventory_management_system.Models;

public class Purchase
{
    public int Id { get; set; }

    public int SupplierId { get; set; }

    public int WarehouseId { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<PurchaseItem> Items { get; set; } = new();

    public decimal TotalAmount => Items.Sum(x => x.Total);
}