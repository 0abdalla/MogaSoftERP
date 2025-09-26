
namespace mogaERP.Infrastructure._Data.Configurations;
public class RestrictionTypeConfiguration : IEntityTypeConfiguration<RestrictionType>
{
    public void Configure(EntityTypeBuilder<RestrictionType> builder)
    {
        builder.ToTable("RestrictionTypes", SchemaNames.Accounting);
        builder.Property(x => x.Name).HasMaxLength(256);
        builder.Property(x => x.Description).HasMaxLength(1000);
    }
}
