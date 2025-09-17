using mogaERP.Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mogaERP.Domain.Entities;
[Table("AccountingGuidances", Schema = SchemaNames.Accounting)]
public class AccountingGuidance : BaseEntity
{
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;
}
