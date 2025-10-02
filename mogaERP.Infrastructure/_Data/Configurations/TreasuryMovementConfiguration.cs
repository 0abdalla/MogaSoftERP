
namespace mogaERP.Infrastructure._Data.Configurations;
public class TreasuryMovementConfiguration : IEntityTypeConfiguration<TreasuryMovement>
{
    public void Configure(EntityTypeBuilder<TreasuryMovement> builder)
    {
        builder.ToTable("TreasuryMovements", SchemaNames.Accounting);


    }
}
