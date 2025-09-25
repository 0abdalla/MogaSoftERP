
namespace mogaERP.Infrastructure._Data.Configurations;
public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches", SchemaNames.MasterData);

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Location)
            .HasMaxLength(300);

        builder.Property(b => b.ContactNumber)
            .HasMaxLength(50);

        builder.Property(b => b.Email)
            .HasMaxLength(100);
    }
}
