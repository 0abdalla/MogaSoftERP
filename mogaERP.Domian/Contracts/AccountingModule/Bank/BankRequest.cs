namespace mogaERP.Domain.Contracts.AccountingModule.Bank;
public class BankRequest
{
    public string Name { get; set; }
    public string? Code { get; set; }
    public string? AccountNumber { get; set; }
    public string Currency { get; set; }
    public decimal InitialBalance { get; set; }
}
