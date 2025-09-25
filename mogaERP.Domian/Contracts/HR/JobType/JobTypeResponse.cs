using mogaERP.Domain.Contracts.Common;

namespace mogaERP.Domain.Contracts.HR.JobType;
public class JobTypeResponse : AuditResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; }
}