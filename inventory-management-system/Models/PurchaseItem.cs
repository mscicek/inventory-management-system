namespace inventory_management_system.Models;

public class PurchaseItem
{
    public int Id { get; set; }

    public int PurchaseId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Total => Quantity * UnitPrice;
}