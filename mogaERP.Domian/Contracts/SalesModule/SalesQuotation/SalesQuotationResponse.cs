namespace mogaERP.Domain.Contracts.SalesModule.SalesQuotation
{
    public class SalesQuotationResponse
    {
        public int Id { get; set; }
        public string QuotationNumber { get; set; } 
        public DateTime QuotationDate { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; } 
        public string? Description { get; set; }
        public DateOnly ValidUntil { get; set; }
        public bool IsTaxIncluded { get; set; }

        public List<SalesQuotationItemResponse> Items { get; set; } = [];
        public List<PaymentTermResponse> PaymentTerms { get; set; } = [];

        public decimal TotalItemsPrice { get; set; }

    }

    public class SalesQuotationItemResponse
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class PaymentTermResponse
    {
        public string Condition { get; set; } = string.Empty;
        public decimal Percentage { get; set; }
    }

}
