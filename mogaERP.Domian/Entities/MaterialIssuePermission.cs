using System.ComponentModel.DataAnnotations;

namespace mogaERP.Domain.Entities;
public class MaterialIssuePermission : BaseEntity
{
    public string PermissionNumber { get; set; } = string.Empty;
    public DateOnly PermissionDate { get; set; }
    public string? DocumentNumber { get; set; }
    public string? Notes { get; set; }


    public int StoreId { get; set; }
    public int? JobDepartmentId { get; set; }
    public int? DisbursementRequestId { get; set; }
    public int? DailyRestrictionId { get; set; }


    public DisbursementRequest? DisbursementRequest { get; set; } = default!;
    public Store Store { get; set; } = default!;
    public DailyRestriction DailyRestriction { get; set; } = default!;
    public JobDepartment? JobDepartment { get; set; } = default!;
    public ICollection<MaterialIssueItem> Items { get; set; } = new List<MaterialIssueItem>();
}

public class MaterialIssueItem
{
    public int Id { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    [MaxLength(100)]
    public string Unit { get; set; } = string.Empty;


    public int ItemId { get; set; }
    public int MaterialIssuePermissionId { get; set; }
    public Item Item { get; set; } = default!;
    public MaterialIssuePermission MaterialIssuePermission { get; set; } = default!;
}