
namespace mogaERP.Infrastructure._Data.Configurations;
public class JobTitleConfiguration : IEntityTypeConfiguration<JobTitle>
{
    public void Configure(EntityTypeBuilder<JobTitle> builder)
    {
        builder.ToTable("JobTitles", SchemaNames.HR);

        builder.Property(x => x.Name).HasMaxLength(250);
        builder.Property(x => x.Description).HasMaxLength(1000);
    }
}
