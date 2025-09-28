namespace mogaERP.Domain.Contracts.HR.Staff;
public class StaffCountsResponse
{
    public Dictionary<string, int> JobTitleCounts { get; set; } = new();
}
