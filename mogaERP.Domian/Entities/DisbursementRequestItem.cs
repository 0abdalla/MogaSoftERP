namespace mogaERP.Domain.Entities;
public class DisbursementRequestItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public int DisbursementRequestId { get; set; }
    public int ItemId { get; set; }

    public DisbursementRequest DisbursementRequest { get; set; } = default!;
    public Item Item { get; set; } = default!;
}
