namespace mogaERP.Domain.Entities;
public class ReceiptPermission : BaseEntity
{
    public string PermissionNumber { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateOnly PermissionDate { get; set; }
    public int StoreId { get; set; }
    public int SupplierId { get; set; }
    public int PurchaseOrderId { get; set; }
    public string? Notes { get; set; }

    public Supplier Supplier { get; set; } = default!;
    public Store Store { get; set; } = default!;

    public PurchaseOrder PurchaseOrder { get; set; } = default!;
    public ICollection<ReceiptPermissionItem> Items { get; set; } = new HashSet<ReceiptPermissionItem>();

    public DailyRestriction DailyRestriction { get; set; } = default!;
    public int? DailyRestrictionId { get; set; }


    // public PurchasePermissionStatus Status { get; set; } = PurchasePermissionStatus.Draft;
}
