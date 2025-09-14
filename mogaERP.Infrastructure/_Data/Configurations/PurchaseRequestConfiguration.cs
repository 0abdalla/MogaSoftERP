
namespace mogaERP.Infrastructure._Data.Configurations;
public class PurchaseRequestConfiguration : IEntityTypeConfiguration<PurchaseRequest>
{
    public void Configure(EntityTypeBuilder<PurchaseRequest> builder)
    {
        builder.ToTable("PurchaseRequests", SchemaNames.Procurement);

        builder.Property(pr => pr.RequestNumber)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(pr => pr.Notes)
               .IsRequired()
               .HasMaxLength(750);

        builder.Property(pr => pr.Purpose)
               .IsRequired()
               .HasMaxLength(350);

        builder.HasOne(pr => pr.AwardedQuotation)
          .WithMany()
          .HasForeignKey(pr => pr.AwardedQuotationId)
          .OnDelete(DeleteBehavior.Restrict);
    }
}
