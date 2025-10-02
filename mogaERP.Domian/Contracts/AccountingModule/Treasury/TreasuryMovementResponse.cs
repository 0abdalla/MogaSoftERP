namespace mogaERP.Domain.Contracts.AccountingModule.Treasury;
public class TreasuryMovementResponse
{
    public int Id { get; set; }
    public int TreasuryId { get; set; }
    public string TreasuryName { get; set; } = string.Empty;
    public int TreasuryNumber { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal ClosingBalance { get; set; }
    public DateOnly OpenedIn { get; set; }
    public DateOnly? ClosedIn { get; set; }
    public bool IsClosed { get; set; }
    public decimal TotalCredits { get; set; }
    public decimal TotalDebits { get; set; }
    public decimal Balance { get; set; }
    public List<TransactionDetail> Transactions { get; set; } = new();
}
public class TransactionDetail
{
    public string DocumentId { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ReceivedFrom { get; set; } = string.Empty;
    public decimal Credit { get; set; }
    public decimal Debit { get; set; }
    public int? AccountId { get; set; }
}
