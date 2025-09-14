namespace mogaERP.Domain.Entities;
public class PurchaseRequestItem
{
    public int Id { get; set; }
    public decimal Quantity { get; set; }
    public string? Notes { get; set; }


    public int PurchaseRequestId { get; set; }
    public int ItemId { get; set; }
    public PurchaseRequest PurchaseRequest { get; set; } = default!;
    public Item Item { get; set; } = default!;
}
