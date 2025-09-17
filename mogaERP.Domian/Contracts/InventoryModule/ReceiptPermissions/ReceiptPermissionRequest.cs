namespace mogaERP.Domain.Contracts.InventoryModule.ReceiptPermissions;
public class ReceiptPermissionRequest
{
    public string DocumentNumber { get; set; }
    public DateOnly PermissionDate { get; set; }
    public List<ReceiptPermissionItemRequest> Items { get; set; } = [];
    public int StoreId { get; set; }
    public int SupplierId { get; set; }
    public int PurchaseOrderId { get; set; }
    public string? Notes { get; set; }
}

public class ReceiptPermissionItemRequest
{
    public int ItemId { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

