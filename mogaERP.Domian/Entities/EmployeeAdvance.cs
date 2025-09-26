namespace mogaERP.Domain.Entities;

public class EmployeeAdvance : BaseEntity
{
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
    public int StaffId { get; set; }
    public Staff Staff { get; set; } = default!;
}
