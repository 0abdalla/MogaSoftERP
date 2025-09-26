
namespace mogaERP.Infrastructure._Data.Configurations;
public class JobTypeConfiguration : IEntityTypeConfiguration<JobType>
{
    public void Configure(EntityTypeBuilder<JobType> builder)
    {
        builder.ToTable("JobTypes", SchemaNames.HR);

        builder.Property(x => x.Name).HasMaxLength(250);
        builder.Property(x => x.Description).HasMaxLength(1000);
    }
}
