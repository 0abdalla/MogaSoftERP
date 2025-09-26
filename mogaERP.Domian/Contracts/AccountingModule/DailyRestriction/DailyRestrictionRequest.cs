namespace mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;
public class DailyRestrictionRequest
{
    public DateOnly RestrictionDate { get; set; }
    public int RestrictionTypeId { get; set; }
    //public string? LedgerNumber { get; set; }
    public string? Description { get; set; }
    public int? AccountingGuidanceId { get; set; }
    public List<DailyRestrictionDetailRequest> Details { get; set; } = new();
}

public class DailyRestrictionDetailRequest
{
    public int AccountId { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public int? CostCenterId { get; set; }
    public string? Note { get; set; }
}
