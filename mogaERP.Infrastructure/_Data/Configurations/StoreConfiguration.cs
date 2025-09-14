using mogaERP.Infrastructure.Constants;

namespace mogaERP.Infrastructure._Data.Configurations;
public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.ToTable("Stores", SchemaNames.Inventory);

        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(s => s.Code)
                .HasMaxLength(50);

        builder.Property(s => s.Address)
                .HasMaxLength(300);

        builder.Property(s => s.PhoneNumber)
                .HasMaxLength(20);
    }
}
