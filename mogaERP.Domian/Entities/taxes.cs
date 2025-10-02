using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mogaERP.Domain.Entities
{
    public class Tax : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Percentage { get; set; }
    }

}
