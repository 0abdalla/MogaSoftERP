using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mogaERP.Domain.Contracts.SalesModule.SalesInvoices
{
    public class SalesInvoiceResponse
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public int? QuotationId { get; set; }
        public string? QuotationNumber { get; set; }

        public int RevenueTypeId { get; set; }
        public string RevenueTypeName { get; set; } = string.Empty;

        public int? TaxId { get; set; }
        public string? TaxName { get; set; }
        public decimal? TaxPercentage { get; set; }

        public bool IsTaxIncluded { get; set; }

        public List<SalesInvoiceItemResponse> Items { get; set; } = [];
        public decimal TotalAmount { get; set; }
    }

    public class SalesInvoiceItemResponse
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
