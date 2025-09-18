namespace mogaERP.Domain.Contracts.InventoryModule.DisbursementRequest;
public class DisbursementRequestRequest
{
    public DateOnly Date { get; set; }
    public string? Notes { get; set; }
    public int? JobDepartmentId { get; set; }
    public List<DisbursementRequestItemRequest> Items { get; set; } = [];
}

public class DisbursementRequestItemRequest
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}
