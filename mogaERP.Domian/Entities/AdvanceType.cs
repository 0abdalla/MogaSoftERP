using mogaERP.Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mogaERP.Domain.Entities;
[Table("AdvanceTypes", Schema = SchemaNames.HR)]
public class AdvanceType
{
    [Key]
    public int AdvanceTypeId { get; set; }

    [MaxLength(250)]
    public string NameAR { get; set; }


    [MaxLength(250)]
    public string NameEN { get; set; }


    [MaxLength(250)]
    public string Code { get; set; }


    [MaxLength(750)]
    public string Notes { get; set; }

    public bool IsActive { get; set; } = true;
}
