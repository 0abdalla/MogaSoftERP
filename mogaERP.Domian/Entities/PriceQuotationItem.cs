namespace mogaERP.Domain.Entities;
public class PriceQuotationItem
{
    public int Id { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total => Quantity * UnitPrice;
    public string? Notes { get; set; }


    public int PriceQuotationId { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; } = default!;
    public PriceQuotation PriceQuotation { get; set; } = default!;
}
