using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mogaERP.Services.Specifications
{
    public class SalesInvoiceSpecification : BaseSpecification<Invoice>
    {
        public SalesInvoiceSpecification(SearchRequest request, bool asNoTracking = true)
            : base(x => !x.IsDeleted)
        {
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim();

                AddCriteria(x =>
                    (x.InvoiceNumber != null && x.InvoiceNumber.Contains(term)) ||
                    (x.Customer != null && x.Customer.Name.Contains(term))
                );
            }

            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (request.SortDescending)
                    ApplyOrderByDescending(e => EF.Property<object>(e, request.SortBy));
                else
                    ApplyOrderBy(e => EF.Property<object>(e, request.SortBy));
            }
            else
            {
                ApplyOrderByDescending(x => x.Id);
            }

            ApplyPagination((request.PageNumber - 1) * request.PageSize, request.PageSize);

            if (asNoTracking)
                ApplyAsNoTracking();

            AddIncludes();
        }

        public SalesInvoiceSpecification(int id)
            : base(x => x.Id == id && !x.IsDeleted)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(x => x.Customer);
            Includes.Add(x => x.Quotation);
            Includes.Add(x => x.RevenueType);
            Includes.Add(x => x.Tax);
            Includes.Add(x => x.Items);

            AddInclude("Items.Item");
        }
    }
}
