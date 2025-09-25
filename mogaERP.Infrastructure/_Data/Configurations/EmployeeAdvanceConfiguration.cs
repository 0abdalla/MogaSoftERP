
namespace mogaERP.Infrastructure._Data.Configurations;
public class EmployeeAdvanceConfiguration : IEntityTypeConfiguration<EmployeeAdvance>
{
    public void Configure(EntityTypeBuilder<EmployeeAdvance> builder)
    {
        builder.ToTable("EmployeeAdvances", SchemaNames.HR);

        builder.Property(e => e.AdvanceName)
            .HasMaxLength(200);

        builder.Property(e => e.AdvanceAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(e => e.PaymentAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(e => e.Benefit)
            .HasPrecision(18, 2);

        builder.Property(e => e.Notes)
            .HasMaxLength(1000);
    }
}
