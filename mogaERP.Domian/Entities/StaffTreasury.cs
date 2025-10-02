using mogaERP.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace mogaERP.Domain.Entities;
[Table("StaffTreasuries", Schema = SchemaNames.Accounting)]
public class StaffTreasury
{
    public int Id { get; set; }
    public int StaffId { get; set; }
    public Staff Staff { get; set; } = default!;

    public int TreasuryId { get; set; }
    public Treasury Treasury { get; set; } = default!;
}
