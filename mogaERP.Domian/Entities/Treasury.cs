namespace mogaERP.Domain.Entities;
public class Treasury : BaseEntity
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Currency { get; set; }
    public decimal OpeningBalance { get; set; }

    public int? BranchId { get; set; }
    public Branch? Branch { get; set; } = default!;
}
