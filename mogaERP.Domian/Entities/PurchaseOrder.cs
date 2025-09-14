using mogaERP.Domain.Enums;

namespace mogaERP.Domain.Entities;
public class PurchaseOrder : BaseEntity
{
    public string OrderNumber { get; set; } = string.Empty;
    public string? ReferenceNumber { get; set; }
    public DateOnly OrderDate { get; set; }
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = default!;
    public string? Description { get; set; }
    public PurchaseStatus Status { get; set; } = PurchaseStatus.Pending;

    public int? PurchaseRequestId { get; set; }
    public PurchaseRequest? PurchaseRequest { get; set; }
    public ICollection<PurchaseOrderItem> Items { get; set; } = new HashSet<PurchaseOrderItem>();
}
