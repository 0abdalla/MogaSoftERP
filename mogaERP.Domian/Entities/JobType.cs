namespace mogaERP.Domain.Entities;
public class JobType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public StatusTypes Status { get; set; }

    public ICollection<Staff> Staffs { get; set; } = new HashSet<Staff>();
}
