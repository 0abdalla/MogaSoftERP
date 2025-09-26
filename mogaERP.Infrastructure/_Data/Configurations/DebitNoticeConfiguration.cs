
namespace mogaERP.Infrastructure._Data.Configurations;
public class DebitNoticeConfiguration : IEntityTypeConfiguration<DebitNotice>
{
    public void Configure(EntityTypeBuilder<DebitNotice> builder)
    {
        builder.ToTable("DebitNotices", SchemaNames.Accounting);
        builder.Property(x => x.CheckNumber).HasMaxLength(250);
        builder.Property(x => x.Notes).HasMaxLength(1000);
    }
}
