namespace mogaERP.Domain.Entities;
public class ReceiptPermissionItem
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; } = default!;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public DateOnly? ExpiryDate { get; set; }

    public ReceiptPermission? ReceiptPermission { get; set; } = default!;
    public int? ReceiptPermissionId { get; set; }
}
