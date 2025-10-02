namespace mogaERP.Domain.Contracts.SalesModule.SalesQuotation
{
    public class SalesQuotationRequest
    {
        public DateTime QuotationDate { get; set; }
        public int CustomerId { get; set; }
        public string ValidityPeriod { get; set; } = "7Days";
        public bool IsTaxIncluded { get; set; }

        public string? Description { get; set; }
        public List<SalesQuotationItemRequest> Items { get; set; } = [];
        public List<PaymentTermRequest> PaymentTerms { get; set; } = [];

    }



    public class SalesQuotationItemRequest
    {
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class PaymentTermRequest
    {
        public string Condition { get; set; } = string.Empty;
        public decimal Percentage { get; set; }
    }
}
