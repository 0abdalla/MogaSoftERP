namespace mogaERP.Infrastructure._Data.Configurations;
public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("PurchaseOrders", schema: SchemaNames.Procurement);

        builder.Property(po => po.OrderNumber)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(po => po.ReferenceNumber)
            .HasMaxLength(50);

        builder.Property(po => po.Description)
            .HasMaxLength(500);
    }
}
