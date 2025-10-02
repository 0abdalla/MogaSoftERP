namespace mogaERP.Domain.Entities;
public class TreasuryMovement : BaseEntity
{
    public int TreasuryNumber { get; set; }
    public decimal OpeningBalance { get; set; } = 0M;
    public DateOnly OpenedIn { get; set; }
    public DateOnly? ClosedIn { get; set; }
    public bool IsClosed { get; set; } = false;
    public decimal TotalCredits { get; set; }
    public decimal TotalDebits { get; set; }
    public decimal Balance { get; set; }

    public int TreasuryId { get; set; }
    public Treasury Treasury { get; set; } = default!;
    public bool IsReEnabled { get; set; } = false;
}
