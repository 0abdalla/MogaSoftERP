using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mogaERP.Domain.Contracts.SalesModule.SalesQuotation
{
    public class QuotationToReturnResponse
    {
        public string QuotationNumber { get; set; }
        public decimal TotalItemsPrice { get; set; }
    }

}
