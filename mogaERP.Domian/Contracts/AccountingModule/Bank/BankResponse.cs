using mogaERP.Domain.Contracts.Common;

namespace mogaERP.Domain.Contracts.AccountingModule.Bank;
public class BankResponse : AuditResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Code { get; set; }
    public string? AccountNumber { get; set; }
    public string Currency { get; set; }
    public decimal InitialBalance { get; set; }
}
