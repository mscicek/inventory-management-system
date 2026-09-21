namespace inventory_management_system.Models;

public class StockMovement
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int WarehouseId { get; set; }

    public int Quantity { get; set; }

    public StockMovementType Type { get; set; }

    public string ReferenceType { get; set; } = string.Empty;

    public int? ReferenceId { get; set; }

    public DateTime CreatedAt { get; set; }
}