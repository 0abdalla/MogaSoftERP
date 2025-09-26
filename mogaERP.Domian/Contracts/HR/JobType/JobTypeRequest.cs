namespace mogaERP.Domain.Contracts.HR.JobType;
public class JobTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "Active";
}