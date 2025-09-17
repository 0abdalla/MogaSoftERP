namespace mogaERP.Domain.Contracts.AccountingModule;
public class PartialDailyRestrictionResponse
{
    public int? Id { get; set; }
    public string? RestrictionNumber { get; set; }
    public DateOnly RestrictionDate { get; set; }
    public string? AccountingGuidanceName { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
    public decimal Amount { get; set; }
    public string? Number { get; set; }
}
