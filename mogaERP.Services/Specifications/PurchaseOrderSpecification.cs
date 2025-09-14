namespace mogaERP.Services.Specifications;
public class PurchaseOrderSpecification : BaseSpecification<PurchaseOrder>
{
    public PurchaseOrderSpecification(SearchRequest request, bool asNoTracking = false)
    {
        AddIncludes();

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            AddCriteria(x => x.OrderNumber.Contains(request.SearchTerm) ||
                             x.Supplier.Name.Contains(request.SearchTerm));
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

    public PurchaseOrderSpecification(int id)
        : base(x => x.Id == id && !x.IsDeleted)
    {
        AddIncludes();
    }

    private void AddIncludes()
    {
        Includes.Add(x => x.Supplier);
        Includes.Add(x => x.PurchaseRequest);
        Includes.Add(x => x.Items);

        AddInclude("Items.Item");
    }
}

