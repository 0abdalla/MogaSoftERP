
namespace mogaERP.Infrastructure._Data.Configurations;
public class JobDepartmentConfiguration : IEntityTypeConfiguration<JobDepartment>
{
    public void Configure(EntityTypeBuilder<JobDepartment> builder)
    {
        builder.ToTable("JobDepartments", SchemaNames.HR);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(750);
    }
}
