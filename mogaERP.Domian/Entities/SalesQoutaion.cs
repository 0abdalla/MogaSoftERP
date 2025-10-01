namespace mogaERP.Domain.Entities
{
    public class SalesQuotation : BaseEntity
    {
        public string QuotationNumber { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateOnly ValidUntil { get; set; }
        public List<PaymentTerm> PaymentTerms { get; set; } = new();
        public bool IsTaxIncluded { get; set; }
        public ICollection<QuotationItem> Items { get; set; } = [];

        public Invoice? Invoice { get; set; }
    }
    public class QuotationItem : BaseEntity
    {
        public int QuotationId { get; set; }
        public int ItemId { get; set; }
        public Item? Item { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class PaymentTerm : BaseEntity
    {
        public decimal Percentage { get; set; }
        public string Condition { get; set; } = string.Empty;
    }

    public class Invoice : BaseEntity
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public int QuotationId { get; set; }
        public SalesQuotation? Quotation { get; set; }
        public ICollection<InvoiceItem> Items { get; set; } = [];
        public bool IsTaxIncluded { get; set; }
    }

    public class InvoiceItem : BaseEntity
    {
        public int InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }
        public int ItemId { get; set; }
        public Item? Item { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
