namespace mogaERP.Services.Specifications;
public class ReceiptPermissionSpecification : BaseSpecification<ReceiptPermission>
{
    public ReceiptPermissionSpecification(SearchRequest request, bool asNoTracking = false)
        : base(x => !x.IsDeleted)
    {

        AddIncludes();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            AddCriteria(x =>
                !x.IsDeleted &&
                (
                    x.PermissionNumber.Contains(request.SearchTerm) ||
                    (x.Notes != null && x.Notes.Contains(request.SearchTerm))
              ));
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            if (request.SortDescending)
                ApplyOrderByDescending(e => EF.Property<object>(e, request.SortBy));
            else
                ApplyOrderBy(e => EF.Property<object>(e, request.SortBy));
        }

        ApplyOrderByDescending(x => x.PermissionDate);

        ApplyPagination((request.PageNumber - 1) * request.PageSize, request.PageSize);

        if (asNoTracking)
            ApplyAsNoTracking();
    }

    public ReceiptPermissionSpecification(int id)
        : base(x => x.Id == id && !x.IsDeleted)
    {
        AddIncludes();
    }


    private void AddIncludes()
    {
        AddInclude(x => x.CreatedBy);
        AddInclude(x => x.UpdatedBy);

        AddInclude(x => x.Store);
        AddInclude(x => x.Supplier);
        AddInclude(x => x.PurchaseOrder);
        AddInclude(x => x.DailyRestriction);
        AddInclude(x => x.Items);

        AddInclude("Items.Item");
        AddInclude("Items.Item.Unit");

    }
}
