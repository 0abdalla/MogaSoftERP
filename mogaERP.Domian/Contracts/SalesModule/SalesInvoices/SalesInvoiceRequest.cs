using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mogaERP.Domain.Contracts.SalesModule.SalesInvoices
{
    public class SalesInvoiceRequest
    {
        public DateTime InvoiceDate { get; set; }
        public int CustomerId { get; set; }

        public int? QuotationId { get; set; }
        public int RevenueTypeId { get; set; } 
        public int TaxId { get; set; }
        public bool IsTaxIncluded { get; set; }

        public List<SalesInvoiceItemRequest> Items { get; set; } = [];
    }

    public class SalesInvoiceItemRequest
    {
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
