using mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;

namespace mogaERP.Domain.Contracts.InventoryModule.ReceiptPermissions;
public class ReceiptPermissionResponse
{
    public int Id { get; set; }
    public string PermissionNumber { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateOnly PermissionDate { get; set; }
    public string? Notes { get; set; }
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public int StoreId { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<ReceiptPermissionItemResponse> Items { get; set; } = new();

    public int? PurchaseOrderId { get; set; }
    public string? PurchaseOrderNumber { get; set; }

    public PartialDailyRestrictionResponse DailyRestriction { get; set; } = new();
}

public class ReceiptPermissionItemResponse
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int? UnitId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

