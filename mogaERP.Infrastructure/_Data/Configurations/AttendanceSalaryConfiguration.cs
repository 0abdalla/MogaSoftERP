
namespace mogaERP.Infrastructure._Data.Configurations;
public class AttendanceSalaryConfiguration : IEntityTypeConfiguration<AttendanceSalary>
{
    public void Configure(EntityTypeBuilder<AttendanceSalary> builder)
    {
        builder.ToTable("AttendanceSalaries", SchemaNames.HR);

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .HasMaxLength(250);

        builder.Property(a => a.WorkDays)
            .HasPrecision(10, 2);

        builder.Property(a => a.TotalFingerprintHours)
            .HasPrecision(10, 2);

        builder.Property(a => a.SickDays)
            .HasPrecision(10, 2);

        builder.Property(a => a.OtherDays)
            .HasPrecision(10, 2);
    }
}
