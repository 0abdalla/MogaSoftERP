
namespace mogaERP.Infrastructure._Data.Configurations;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers", SchemaNames.Sales);

        builder.Property(x => x.Address).HasMaxLength(550);
        builder.Property(x => x.Name).HasMaxLength(750);
        builder.Property(x => x.AccountCode).HasMaxLength(550);
        builder.Property(x => x.CommercialRegistration).HasMaxLength(550);
        builder.Property(x => x.TaxNumber).HasMaxLength(250);
        builder.Property(x => x.PhoneNumber).HasMaxLength(50);
    }
}
