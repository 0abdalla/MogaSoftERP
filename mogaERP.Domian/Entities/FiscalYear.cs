using mogaERP.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace mogaERP.Domain.Entities;

[Table("FiscalYears", Schema = SchemaNames.Accounting)]
public class FiscalYear : BaseEntity
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
