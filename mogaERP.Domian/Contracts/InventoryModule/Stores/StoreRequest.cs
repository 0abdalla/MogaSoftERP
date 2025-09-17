namespace mogaERP.Domain.Contracts.InventoryModule.Stores;
public class StoreRequest
{
    public string Name { get; set; }
    public string? Code { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }

    public int? StoreTypeId { get; set; }
}