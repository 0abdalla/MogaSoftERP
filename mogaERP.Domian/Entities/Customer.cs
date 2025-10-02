namespace mogaERP.Domain.Entities;
public class Customer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? AccountCode { get; set; }

    public string? Address { get; set; } // العنوان 
    public string? PhoneNumber { get; set; } // هاتف العميل
    public string? TaxNumber { get; set; } // الرقم الضريبي
    public string? CommercialRegistration { get; set; } // السجل التجاري 
    public string? Email { get; set; }


    public PaymentType PaymentType { get; set; } = PaymentType.Cash;
    public decimal CreditLimit { get; set; } = 0;

    //public int CurrencyId { get; set; }
    //public Currency Currency { get; set; } = null!;
    // public ICollection<CustomerInvoice> Invoices { get; set; } = new HashSet<CustomerInvoice>();
    //public ICollection<CustomerPayment> Payments { get; set; } = new HashSet<CustomerPayment>();
}
