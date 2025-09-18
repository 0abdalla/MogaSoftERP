namespace mogaERP.Domain.Entities;
public class JobDepartment : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public StatusTypes Status { get; set; } = StatusTypes.Active;
}
