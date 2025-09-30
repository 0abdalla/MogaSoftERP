using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mogaERP.Domain.Entities;
using mogaERP.Domain.Constants;

namespace mogaERP.Infrastructure._Data.Configurations
{
    public class SalesQuotationConfiguration : IEntityTypeConfiguration<SalesQuotation>
    {
        public void Configure(EntityTypeBuilder<SalesQuotation> builder)
        {
            builder.ToTable("SalesQuotations", schema: SchemaNames.Sales);

            builder.Property(q => q.QuotationNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(q => q.IsTaxIncluded)
                .IsRequired();

         
            
        }
    }

    public class QuotationItemConfiguration : IEntityTypeConfiguration<QuotationItem>
    {
        public void Configure(EntityTypeBuilder<QuotationItem> builder)
        {
            builder.ToTable("QuotationItems", schema: SchemaNames.Sales);

            builder.Property(qi => qi.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(qi => qi.Quantity).HasColumnType("decimal(18,2)");
        }
    }

    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices", schema: SchemaNames.Sales);

            builder.Property(i => i.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);

             }
    }

    public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceItem> builder)
        {
            builder.ToTable("InvoiceItems", schema: SchemaNames.Sales);

            builder.Property(ii => ii.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(ii => ii.Quantity).HasColumnType("decimal(18,2)");
        }
    }

    public class PaymentTermConfiguration : IEntityTypeConfiguration<PaymentTerm>
    {
        public void Configure(EntityTypeBuilder<PaymentTerm> builder)
        {
            builder.ToTable("PaymentTerms", schema: SchemaNames.Sales);
            builder.Property(pt => pt.Percentage).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(pt => pt.Condition).HasMaxLength(200).IsRequired();
        }
    }
}
