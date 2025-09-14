namespace mogaERP.Domain.Contracts.ProcurementModule.PurchaseOrder;
public class PurchaseOrderRequest
{
    public string? ReferenceNumber { get; set; }
    public DateOnly OrderDate { get; set; }
    public int SupplierId { get; set; }
    public string? Description { get; set; }
    public int PurchaseRequestId { get; set; }
    public List<PurchaseOrderItemRequest> Items { get; set; } = [];
}

public class PurchaseOrderItemRequest
{
    public int ItemId { get; set; }
    public decimal RequestedQuantity { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
