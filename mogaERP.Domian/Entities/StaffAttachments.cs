using mogaERP.Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mogaERP.Domain.Entities;

[Table("StaffAttachments", Schema = SchemaNames.HR)]
public class StaffAttachments : BaseEntity
{
    [MaxLength(1000)]
    public string FileUrl { get; set; } = string.Empty;

    public int StaffId { get; set; }
    public Staff Staff { get; set; } = default!;
}
