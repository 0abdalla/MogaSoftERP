namespace mogaERP.Domain.Entities;
public class Staff : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Email { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public DateOnly HireDate { get; set; }
    public string? NationalId { get; set; }
    public string? Address { get; set; }
    public string? Notes { get; set; }


    public StaffStatus Status { get; set; } = StaffStatus.Active;
    public MaritalStatus? MaritalStatus { get; set; }
    public Gender Gender { get; set; }


    public int? JobTitleId { get; set; }
    public int? JobTypeId { get; set; }
    public int? JobLevelId { get; set; }
    public int? JobDepartmentId { get; set; }
    public int? BranchId { get; set; }

    public decimal? BasicSalary { get; set; }
    public int? Tax { get; set; }
    public int? Insurance { get; set; }
    public int? AnnualDays { get; set; }
    public string? VisaCode { get; set; }
    public decimal Allowances { get; set; } // البدلات
    public decimal? NetSalary { get; set; }
    public decimal? Deductions { get; set; }


    public bool IsAuthorized { get; set; }

    public JobTitle? JobTitle { get; set; } = default!;
    public JobType? JobType { get; set; } = default!;
    public JobLevel? JobLevel { get; set; } = default!;
    public JobDepartment? JobDepartment { get; set; }
    public Branch? Branch { get; set; }
    public ICollection<StaffAttachments> StaffAttachments { get; set; } = new HashSet<StaffAttachments>();
    public ICollection<EmployeeAdvance> EmployeeAdvances { get; set; } = new HashSet<EmployeeAdvance>();
    public ICollection<AttendanceSalary> AttendanceSalaries { get; set; } = new HashSet<AttendanceSalary>();
}
