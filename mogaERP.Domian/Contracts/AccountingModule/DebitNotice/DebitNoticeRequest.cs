namespace mogaERP.Domain.Contracts.AccountingModule.DebitNotice;
public class DebitNoticeRequest
{
    public DateOnly Date { get; set; }
    public int BankId { get; set; }
    public int AccountId { get; set; }
    public string? CheckNumber { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
}
