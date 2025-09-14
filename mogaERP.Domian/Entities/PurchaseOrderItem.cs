using mogaERP.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace mogaERP.Domain.Entities;

[Table("PurchaseOrderItems", Schema = SchemaNames.Procurement)]
public class PurchaseOrderItem
{
    public int Id { get; set; }
    public decimal RequestedQuantity { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public int PurchaseOrderId { get; set; }
    public int ItemId { get; set; }

    public Item Item { get; set; } = default!;
    public PurchaseOrder PurchaseOrder { get; set; } = default!;
}
