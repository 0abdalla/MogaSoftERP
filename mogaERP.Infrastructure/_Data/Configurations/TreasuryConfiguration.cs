
namespace mogaERP.Infrastructure._Data.Configurations;
public class TreasuryConfiguration : IEntityTypeConfiguration<Treasury>
{
    public void Configure(EntityTypeBuilder<Treasury> builder)
    {
        builder.ToTable("Treasuries", SchemaNames.Accounting);

        builder.Property(x => x.Code).HasMaxLength(250);
        builder.Property(x => x.Name).HasMaxLength(450);
        builder.Property(x => x.Currency).HasMaxLength(250);

    }
}
