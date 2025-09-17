
namespace mogaERP.Infrastructure._Data.Configurations;
public class DailyRestrictionConfiguration : IEntityTypeConfiguration<DailyRestriction>
{
    public void Configure(EntityTypeBuilder<DailyRestriction> builder)
    {
        builder.ToTable("DailyRestrictions", schema: SchemaNames.Accounting);


        builder.Property(x => x.RestrictionNumber)
            .HasMaxLength(50);

        builder.Property(x => x.RestrictionDate)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500);

    }
}

public class DailyRestrictionDetailConfiguration : IEntityTypeConfiguration<DailyRestrictionDetail>
{
    public void Configure(EntityTypeBuilder<DailyRestrictionDetail> builder)
    {
        builder.ToTable("DailyRestrictionDetails", SchemaNames.Accounting);

        builder.Property(x => x.Note)
            .HasMaxLength(750);

        builder.Property(x => x.From)
           .HasMaxLength(150);

        builder.Property(x => x.To)
           .HasMaxLength(150);
    }
}
