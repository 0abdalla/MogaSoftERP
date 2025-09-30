namespace mogaERP.Domain.Entities;
public class Branch : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }

    // Navigation properties
    //public ICollection<Treasury> Treasuries { get; set; } = new List<Treasury>();
}
