
namespace mogaERP.Infrastructure._Data.Configurations;
public class DisbursementRequestConfiguration : IEntityTypeConfiguration<DisbursementRequest>
{
    public void Configure(EntityTypeBuilder<DisbursementRequest> builder)
    {
        builder.ToTable("DisbursementRequests", SchemaNames.Inventory);

        builder.Property(x => x.Number)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);
    }
}
