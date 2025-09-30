using mogaERP.Domain.Contracts.Common;

namespace mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;
public class RestrictionTypeResponse : AuditResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
