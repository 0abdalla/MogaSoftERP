using mogaERP.Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mogaERP.Domain.Entities;
[Table("AccountTrees", Schema = SchemaNames.Accounting)]
public class AccountTree
{
    [Key]
    public int CostCenterId { get; set; }
    public string CostCenterNumber { get; set; }
    public string NameAR { get; set; }
    public string NameEN { get; set; }
    public int? ParentId { get; set; }
    public int? CostLevel { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsLocked { get; set; }
    public bool? IsParent { get; set; }
    public bool? IsPost { get; set; }
    public int? IsExpences { get; set; }
    public bool? IsGroup { get; set; }
    public int? DisplayOrder { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
