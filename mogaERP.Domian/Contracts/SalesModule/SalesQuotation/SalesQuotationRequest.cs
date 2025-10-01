namespace mogaERP.Domain.Contracts.SalesModule.SalesQuotation
{
    public class SalesQuotationRequest
    {
        public DateOnly QuotationDate { get; set; }
        public int CustomerId { get; set; }
        public string? Description { get; set; }
        public List<SalesQuotationItemRequest> Items { get; set; } = [];
    }

    public class SalesQuotationItemRequest
    {
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
