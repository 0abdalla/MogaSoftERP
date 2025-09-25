
namespace mogaERP.Infrastructure._Data.Configurations;
public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable("Staff", SchemaNames.HR);

        builder.Property(s => s.FullName)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(s => s.PhoneNumber)
            .HasMaxLength(50);

        // Optional fields with max lengths
        builder.Property(s => s.Code)
            .HasMaxLength(250);

        builder.Property(s => s.Email)
            .HasMaxLength(100);

        builder.Property(s => s.NationalId)
            .HasMaxLength(250);

        builder.Property(s => s.Address)
            .HasMaxLength(500);

        builder.Property(s => s.Notes)
            .HasMaxLength(1000);

        builder.Property(s => s.VisaCode)
            .HasMaxLength(250);

        // Decimal precision configuration
        builder.Property(s => s.BasicSalary)
            .HasPrecision(18, 2);

        builder.Property(s => s.VariableSalary)
            .HasPrecision(18, 2);

        builder.Property(s => s.Allowances)
            .HasPrecision(18, 2);

        builder.Property(s => s.Rewards)
            .HasPrecision(18, 2);
    }
}
