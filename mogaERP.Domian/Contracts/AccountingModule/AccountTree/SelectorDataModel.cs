namespace mogaERP.Domain.Contracts.AccountingModule.AccountTree;
public class SelectorDataModel
{
    public int Id { get; set; }
    public int Value => Id;
    public string Name { get; set; }
    public string Code { get; set; }
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
    public int? VacationDays { get; set; }
}
