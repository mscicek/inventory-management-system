namespace inventory_management_system.Models;

public class WarehouseStock
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int WarehouseId { get; set; }

    public int Quantity { get; set; }
}