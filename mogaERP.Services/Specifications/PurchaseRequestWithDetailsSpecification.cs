
namespace mogaERP.Services.Specifications;
public class PurchaseRequestWithDetailsSpecification : BaseSpecification<PurchaseRequest>
{
    public PurchaseRequestWithDetailsSpecification(int id)
        : base(x => x.Id == id && !x.IsDeleted)
    {
        AddIncludes();
    }

    public PurchaseRequestWithDetailsSpecification(SearchRequest request, bool asNoTracking = false)
        : base(x => !x.IsDeleted)
    {
        AddIncludes();

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            AddCriteria(x => x.RequestNumber.Contains(request.SearchTerm) ||
                             x.Store.Name.Contains(request.SearchTerm));
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            if (request.SortDescending)
                ApplyOrderByDescending(e => EF.Property<object>(e, request.SortBy));
            else
                ApplyOrderBy(e => EF.Property<object>(e, request.SortBy));
        }

        ApplyPagination((request.PageNumber - 1) * request.PageSize, request.PageSize);

        if (asNoTracking)
            ApplyAsNoTracking();
    }


    private void AddIncludes()
    {
        AddInclude(x => x.Store);
        AddInclude(x => x.CreatedBy);
        AddInclude(x => x.UpdatedBy);

        AddInclude(x => x.Items);
        AddInclude("Items.Item");
    }
}
