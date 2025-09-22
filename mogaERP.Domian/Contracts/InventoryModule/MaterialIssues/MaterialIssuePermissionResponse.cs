using mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;
using mogaERP.Domain.Contracts.Common;

namespace mogaERP.Domain.Contracts.InventoryModule.MaterialIssues;
public class MaterialIssuePermissionResponse : AuditResponse
{
    public int Id { get; set; }
    public string PermissionNumber { get; set; }
    public string? DocumentNumber { get; set; }
    public DateOnly PermissionDate { get; set; }
    public int StoreId { get; set; }
    public string StoreName { get; set; }
    public int? JobDepartmentId { get; set; }
    public string? JobDepartmentName { get; set; }
    public string? Notes { get; set; }
    public List<MaterialIssueItemResponse> Items { get; set; } = new();

    public int? DisbursementRequestId { get; set; }
    public string? DisbursementRequestNumber { get; set; }

    public PartialDailyRestrictionResponse DailyRestriction { get; set; } = new();
}

public class MaterialIssueItemResponse
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public string Unit { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
