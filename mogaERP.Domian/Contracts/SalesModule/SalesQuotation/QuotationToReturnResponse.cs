using mogaERP.Domain.Contracts.SalesModule.Customer;

namespace mogaERP.Domain.Contracts.SalesModule.SalesQuotation
{
    public class QuotationToReturnResponse
    {
        public string QuotationNumber { get; set; } = string.Empty;
        public DateTime QuotationDate { get; set; }
        public string? Description { get; set; }
        public string ValidityPeriod { get; set; } = string.Empty;
        public bool IsTaxIncluded { get; set; }
        public List<SalesQuotationItemResponse> Items { get; set; } = new();
        public List<QPaymentTermResponse> PaymentTerms { get; set; } = new();

        public decimal TotalItemsPrice { get; set; }

        public CustomerResponse CustomerData { get; set; } = new();
    }
    public class QuotationItemResponse
    {
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
    public class QPaymentTermResponse
    {
        public string Condition { get; set; } = string.Empty;
        public decimal Percentage { get; set; }
    }


}
