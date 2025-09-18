using mogaERP.Domain.Contracts.Common;

namespace mogaERP.Domain.Contracts.InventoryModule.DisbursementRequest;
public class DisbursementRequestResponse : AuditResponse
{
    public int Id { get; set; }
    public string Number { get; set; }
    public DateOnly Date { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; }
    public int? JobDepartmentId { get; set; }
    public string? JobDepartmentName { get; set; }
    public List<DisbursementItemResponse> Items { get; set; } = new List<DisbursementItemResponse>();
}

public class DisbursementItemResponse
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal PriceAfterTax { get; set; }
    public string? Unit { get; set; }
}

