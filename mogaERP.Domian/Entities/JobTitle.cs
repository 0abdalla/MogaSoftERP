namespace mogaERP.Domain.Entities;
public class JobTitle : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public StatusTypes Status { get; set; }

    public int? JobDepartmentId { get; set; }
    public JobDepartment? JobDepartment { get; set; } = default!;

    public ICollection<Staff> Staffs { get; set; } = new HashSet<Staff>();
}
