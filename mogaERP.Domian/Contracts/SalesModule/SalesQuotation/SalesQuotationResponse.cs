using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mogaERP.Domain.Contracts.SalesModule.SalesQuotation
{
    public class SalesQuotationResponse
    {
        public int Id { get; set; }
        public string QuotationNumber { get; set; } = string.Empty;
        public DateOnly QuotationDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<SalesQuotationItemResponse> Items { get; set; } = [];
    }

    public class SalesQuotationItemResponse
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
