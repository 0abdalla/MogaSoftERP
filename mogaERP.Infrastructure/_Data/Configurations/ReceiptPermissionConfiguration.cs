
namespace mogaERP.Infrastructure._Data.Configurations;
public class ReceiptPermissionConfiguration : IEntityTypeConfiguration<ReceiptPermission>
{
    public void Configure(EntityTypeBuilder<ReceiptPermission> builder)
    {
        builder.ToTable("ReceiptPermissions", schema: SchemaNames.Inventory);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PermissionNumber)
            .HasMaxLength(150);

        builder.Property(x => x.DocumentNumber)
           .HasMaxLength(150);

        builder.Property(x => x.Notes)
            .HasMaxLength(750);
    }
}

public class ReceiptPermissionItemConfiguration : IEntityTypeConfiguration<ReceiptPermissionItem>
{
    public void Configure(EntityTypeBuilder<ReceiptPermissionItem> builder)
    {
        builder.ToTable("ReceiptPermissionItems", schema: SchemaNames.Inventory);
    }
}
