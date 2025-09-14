using mogaERP.Domain.Contracts.Common;

namespace mogaERP.Domain.Contracts.ProcurementModule.PriceQuotation;
public class PriceQuotationRequest
{
    public DateOnly QuotationDate { get; set; }
    public int SupplierId { get; set; }
    public int PurchaseRequestId { get; set; }
    public string? Notes { get; set; }
    public List<PriceQuotationItemRequest> Items { get; set; } = [];
}

public class PriceQuotationResponse : AuditResponse
{
    public int Id { get; set; }
    public string? QuotationNumber { get; set; }
    public DateOnly QuotationDate { get; set; }
    public int SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public string? Notes { get; set; }
    public string? Status { get; set; }
    public decimal TotalAmount { get; set; }

    public int? PurchaseRequestId { get; set; }
    public string? PurchaseRequestNumber { get; set; }

    public List<PriceQuotationItemResponse> Items { get; set; } = [];
}

public class PriceQuotationItemRequest
{
    public int ItemId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Notes { get; set; }
}

public class PriceQuotationItemResponse
{
    public int ItemId { get; set; }
    public string Name { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Unit { get; set; }
    public decimal Total { get; set; }
    public string? Notes { get; set; }
}