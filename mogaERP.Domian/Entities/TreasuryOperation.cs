namespace mogaERP.Domain.Entities;
public class TreasuryOperation
{

    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ReceivedFrom { get; set; } // استلمت من السيد
    public decimal Amount { get; set; } // الوارد
    public bool IsDeleted { get; set; } = false;

    public int? AccountId { get; set; }
    public TransactionType TransactionType { get; set; }
    public int TreasuryId { get; set; }
    public int? TreasuryMovementId { get; set; }

    public Treasury Treasury { get; set; } = default!;
    public TreasuryMovement? TreasuryMovement { get; set; } = default!;
}
