using mogaERP.Domain.Contracts.Common;

namespace mogaERP.Domain.Contracts.HR.JobTitle;
public class JobTitleResponse : AuditResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; }
    public int? JobDepartmentId { get; set; }
    public string? JobDepartmentName { get; set; }
}