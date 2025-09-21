using mogaERP.Domain.Contracts.AccountingModule;
namespace mogaERP.Domain.Contracts.InventoryModule.MaterialIssues;

public class MaterialIssuePermissionToReturnResponse
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string JobDepartmentName { get; set; }
    public string StoreName { get; set; }

    public PartialDailyRestrictionResponse DailyRestriction { get; set; } = new();
}
