using mogaERP.Domain.Contracts.Common;

namespace mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;
public class DailyRestrictionResponse : AuditResponse
{
    public int Id { get; set; }
    public string RestrictionNumber { get; set; }
    public DateOnly RestrictionDate { get; set; }
    public int? RestrictionTypeId { get; set; }
    public string? RestrictionTypeName { get; set; }

    public int? AccountingGuidanceId { get; set; }
    public string? AccountingGuidanceName { get; set; }
    public string? Description { get; set; }


    public List<DailyRestrictionDetailResponse> Details { get; set; } = [];
}
public class DailyRestrictionDetailResponse
{
    public int Id { get; set; }
    public int? AccountId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    public int? CostCenterId { get; set; }
    public string? CostCenterName { get; set; }
    public string? Note { get; set; }
}