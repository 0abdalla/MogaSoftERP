namespace mogaERP.Domain.Entities;
public class AttendanceSalary : BaseEntity
{
    public string? Name { get; set; }           // الاسم
    public int? WorkHours { get; set; }         // عدد الساعات
    public double? WorkDays { get; set; }       // أيام العمل
    public int? RequiredHours { get; set; }     // الساعات المطلوبة
    public double? TotalFingerprintHours { get; set; } // إجمالي ساعات البصمة
    public double? SickDays { get; set; }       // الأيام القلبية
    public double? OtherDays { get; set; }      // أخرى
    public int? Fridays { get; set; }           // أيام الجمع
    public int? TotalDays { get; set; }         // إجمالي الأيام
    public int? Overtime { get; set; }          // الإضافي
    public int? BranchId { get; set; }          // الفرع
    public DateTime? Date { get; set; }          // التاريخ

    public int StaffId { get; set; }
    public Staff Staff { get; set; } = default!;
}
