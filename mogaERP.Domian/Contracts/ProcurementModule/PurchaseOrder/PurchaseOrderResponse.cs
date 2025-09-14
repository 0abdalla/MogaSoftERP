namespace mogaERP.Domain.Contracts.ProcurementModule.PurchaseOrder;
public class PurchaseOrderResponse
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string? ReferenceNumber { get; set; }
    public DateOnly OrderDate { get; set; }
    public int? PurchaseRequestId { get; set; }
    public string? PurchaseRequestNumber { get; set; }

    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; }
    public List<PurchaseOrderItemResponse> Items { get; set; } = [];
}

public class PurchaseOrderItemResponse
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public decimal RequestedQuantity { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
