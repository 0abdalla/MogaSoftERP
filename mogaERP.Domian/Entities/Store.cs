namespace mogaERP.Domain.Entities;
public class Store : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }

    public int? StoreTypeId { get; set; }
    public StoreType StoreType { get; set; } = default!;
}