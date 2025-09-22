
namespace mogaERP.Infrastructure._Data.Configurations;
public class AdditionNoticeConfiguration : IEntityTypeConfiguration<AdditionNotice>
{
    public void Configure(EntityTypeBuilder<AdditionNotice> builder)
    {
        builder.ToTable("AdditionNotices", SchemaNames.Accounting); ;

        builder.Property(an => an.CheckNumber)
            .HasMaxLength(200);

        builder.Property(an => an.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(an => an.Notes)
            .HasMaxLength(1000);

    }
}
