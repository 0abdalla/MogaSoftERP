namespace mogaERP.Domain.Contracts.HR.JobTitle;
public class JobTitleRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "Active";
    public int? JobDepartmentId { get; set; }
}
