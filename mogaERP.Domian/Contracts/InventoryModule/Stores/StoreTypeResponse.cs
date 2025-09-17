using mogaERP.Domain.Contracts.Common;

namespace mogaERP.Domain.Contracts.InventoryModule.Stores;
public class StoreTypeResponse : AuditResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
