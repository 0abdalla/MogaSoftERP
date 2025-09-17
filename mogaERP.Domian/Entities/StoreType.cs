using mogaERP.Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mogaERP.Domain.Entities;
[Table("StoreTypes", Schema = SchemaNames.Inventory)]
public class StoreType : BaseEntity
{
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
