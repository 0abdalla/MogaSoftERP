
namespace mogaERP.Infrastructure._Data.Configurations;
public class MaterialIssuePermissionConfiguration : IEntityTypeConfiguration<MaterialIssuePermission>
{
    public void Configure(EntityTypeBuilder<MaterialIssuePermission> builder)
    {
        builder.ToTable("MaterialIssuePermissions", SchemaNames.Inventory);

        builder.Property(e => e.PermissionNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.DocumentNumber)
            .HasMaxLength(100);

        builder.Property(e => e.Notes)
           .HasMaxLength(850);
    }
}
