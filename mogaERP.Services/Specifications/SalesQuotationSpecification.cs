
namespace mogaERP.Services.Specifications;
public class SalesQuotationSpecification : BaseSpecification<SalesQuotation>
{
    public SalesQuotationSpecification(SearchRequest request, bool asNoTracking = true)
        : base(x => !x.IsDeleted)
    {

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();

            AddCriteria(x =>
                (x.QuotationNumber != null && x.QuotationNumber.Contains(term))
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

        AddIncludes();

        if (asNoTracking)
            ApplyAsNoTracking();

        AddIncludes();
    }

    public SalesQuotationSpecification(int id)
        : base(x => x.Id == id && !x.IsDeleted)
    {
        AddIncludes();

    }

    private void AddIncludes()
    {
        Includes.Add(x => x.Customer);
        Includes.Add(x => x.Invoice);
        Includes.Add(x => x.PaymentTerms);
        Includes.Add(x => x.Items);

        AddInclude("Items.Item");
        AddInclude("Invoice.Quotation");
        AddInclude("Invoice.Items");
        AddInclude("Invoice.Items.Item");
    }
}
