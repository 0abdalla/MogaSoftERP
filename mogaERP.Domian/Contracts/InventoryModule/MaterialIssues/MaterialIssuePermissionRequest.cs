namespace mogaERP.Domain.Contracts.InventoryModule.MaterialIssues;
public class MaterialIssuePermissionRequest
{
    public DateOnly PermissionDate { get; set; }
    public int StoreId { get; set; }
    public int? JobDepartmentId { get; set; }
    public string? Notes { get; set; }
    public int? DisbursementRequestId { get; set; }
    public List<MaterialIssueItemRequest> Items { get; set; } = [];
}

public class MaterialIssueItemRequest
{
    public int ItemId { get; set; }
    public string Unit { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
