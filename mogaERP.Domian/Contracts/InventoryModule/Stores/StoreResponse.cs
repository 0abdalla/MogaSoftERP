using mogaERP.Domain.Contracts.Common;

namespace mogaERP.Domain.Contracts.InventoryModule.Stores;
public class StoreResponse : AuditResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Code { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }

    public int? StoreTypeId { get; set; }
    public string? StoreTypeName { get; set; }
}