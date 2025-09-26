using mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;
using mogaERP.Domain.Contracts.Common;

namespace mogaERP.Domain.Contracts.AccountingModule.AdditionNotice;
public class AdditionNotificationResponse : AuditResponse
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int BankId { get; set; }
    public string BankName { get; set; }
    public int AccountId { get; set; }
    public string AccountName { get; set; }
    public string CheckNumber { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }

    public PartialDailyRestrictionResponse DailyRestriction { get; set; } = new();
}
