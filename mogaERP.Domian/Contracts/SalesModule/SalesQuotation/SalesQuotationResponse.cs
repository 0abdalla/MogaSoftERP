namespace mogaERP.Domain.Contracts.SalesModule.SalesQuotation
{
    public class SalesQuotationResponse
    {
        public int Id { get; set; }
        public string QuotationNumber { get; set; }
        public DateOnly QuotationDate { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Description { get; set; }
        public List<SalesQuotationItemResponse> Items { get; set; } = [];
    }

    public class SalesQuotationItemResponse
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
