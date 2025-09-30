namespace mogaERP.Domain.Contracts.HR.Staff;
public class StaffListResponse
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int? JobTitleId { get; set; }
    public string? JobTitleName { get; set; }

    public int? JobDepartmentId { get; set; }
    public string? JobDepartmentName { get; set; }

    public string Status { get; set; }
}
