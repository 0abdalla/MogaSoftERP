
namespace mogaERP.Infrastructure._Data.Configurations;
public class PriceQuotationConfiguration : IEntityTypeConfiguration<PriceQuotation>
{
    public void Configure(EntityTypeBuilder<PriceQuotation> builder)
    {
        builder.ToTable("PriceQuotations", schema: SchemaNames.Procurement);

        builder.Property(pq => pq.QuotationNumber)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(pq => pq.Notes)
            .HasMaxLength(500);

        builder.HasOne(pq => pq.PurchaseRequest)
           .WithMany(pr => pr.PriceQuotations)
           .HasForeignKey(pq => pq.PurchaseRequestId)
           .OnDelete(DeleteBehavior.Restrict);
    }
}
