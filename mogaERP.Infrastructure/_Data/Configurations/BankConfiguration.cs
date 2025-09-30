
namespace mogaERP.Infrastructure._Data.Configurations;
public class BankConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.ToTable("Banks", SchemaNames.Accounting);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(550);

        builder.Property(b => b.Code)
            .HasMaxLength(200);

        builder.Property(b => b.AccountNumber)
            .HasMaxLength(250);

        builder.Property(b => b.Currency)
            .HasMaxLength(50);

        builder.Property(b => b.InitialBalance)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0M);
    }
}
