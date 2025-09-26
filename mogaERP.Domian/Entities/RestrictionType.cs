namespace mogaERP.Domain.Entities;
public class RestrictionType : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<DailyRestriction> DailyRestrictions { get; set; } = new List<DailyRestriction>();
}
