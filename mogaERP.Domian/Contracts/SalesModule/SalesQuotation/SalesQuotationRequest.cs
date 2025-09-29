using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mogaERP.Domain.Contracts.SalesModule.SalesQuotation
{
    public class SalesQuotationRequest
    {
        public string QuotationNumber { get; set; } = string.Empty;
        public DateOnly QuotationDate { get; set; }
        public int CustomerId { get; set; }
        public string? Description { get; set; }
        public List<SalesQuotationItemRequest> Items { get; set; } = [];
    }

    public class SalesQuotationItemRequest
    {
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
