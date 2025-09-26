
namespace mogaERP.Infrastructure._Data.Configurations;
public class JobLevelConfiguration : IEntityTypeConfiguration<JobLevel>
{
    public void Configure(EntityTypeBuilder<JobLevel> builder)
    {
        builder.ToTable("JobLevels", SchemaNames.HR);

        builder.Property(x => x.Name).HasMaxLength(250);
        builder.Property(x => x.Description).HasMaxLength(1000);
    }
}
