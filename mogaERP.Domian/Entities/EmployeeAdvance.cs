using System.ComponentModel.DataAnnotations;

namespace mogaERP.Domain.Entities;

public class EmployeeAdvance
{
    [Key]
    public int StaffAdvanceId { get; set; }
    public int AdvanceNumber { get; set; }
    public int AdvanceTypeId { get; set; }
    public string? AdvanceName { get; set; }
    public double AdvanceAmount { get; set; }
    public double PaymentAmount { get; set; }
    public double? Benefit { get; set; }
    public DateTime PaymentFromDate { get; set; }
    public DateTime PaymentToDate { get; set; }
    public int? WorkflowStatusId { get; set; }
    public string? Notes { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int StaffId { get; set; }
    public Staff Staff { get; set; } = default!;
}
