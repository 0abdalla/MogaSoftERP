namespace mogaERP.Domain.Contracts.HR.JobLevel;
public class JobLevelRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = "Active";
}
