using mogaERP.Domain.Contracts.Common;

namespace mogaERP.Domain.Contracts.AccountingModule.Treasury;
public class TreasuryResponse : AuditResponse
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
    public string? Currency { get; set; }
    public decimal OpeningBalance { get; set; }
    //public List<PartialMovementResponse> Movements { get; set; } = [];
}
