namespace mogaERP.Domain.Contracts.SalesModule.Customer;
public class CustomerRequest
{
    public string Name { get; set; } = string.Empty;
    public string? AccountCode { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? TaxNumber { get; set; }
    public string? CommercialRegistration { get; set; }
    public string? Email { get; set; }


    public string PaymentType { get; set; } = "Cash";
    public decimal CreditLimit { get; set; } = 0;
}
