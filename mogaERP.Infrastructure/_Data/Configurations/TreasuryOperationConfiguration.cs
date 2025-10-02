namespace mogaERP.Infrastructure._Data.Configurations;
public class TreasuryOperationConfiguration : IEntityTypeConfiguration<TreasuryOperation>
{
    public void Configure(EntityTypeBuilder<TreasuryOperation> builder)
    {
        builder.ToTable("TreasuryOperations", SchemaNames.Accounting);

        builder.Property(x => x.DocumentNumber)
            .HasMaxLength(250);

        builder.Property(x => x.Description)
            .HasMaxLength(550);

        builder.Property(x => x.ReceivedFrom)
            .HasMaxLength(250);
    }
}
