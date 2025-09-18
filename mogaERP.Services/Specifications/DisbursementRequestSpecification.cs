
namespace mogaERP.Services.Specifications;
public class DisbursementRequestSpecification : BaseSpecification<DisbursementRequest>
{
    public DisbursementRequestSpecification(SearchRequest request, bool asNoTracking = true)
        : base(x => !x.IsDeleted)
    {

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            AddCriteria(i => i.Number.Contains(request.SearchTerm));
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            if (request.SortDescending)
                ApplyOrderByDescending(e => EF.Property<object>(e, request.SortBy));
            else
                ApplyOrderBy(e => EF.Property<object>(e, request.SortBy));
        }

        ApplyPagination((request.PageNumber - 1) * request.PageSize, request.PageSize);


        AddIncludes();

        if (asNoTracking)
            ApplyAsNoTracking();

        ApplyOrderByDescending(x => x.Id);

    }

    public DisbursementRequestSpecification(SearchRequest request, PurchaseStatus status, bool asNoTracking = true)
        : base(x => !x.IsDeleted && x.Status == status)
    {

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            AddCriteria(i => i.Number.Contains(request.SearchTerm));
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            if (request.SortDescending)
                ApplyOrderByDescending(e => EF.Property<object>(e, request.SortBy));
            else
                ApplyOrderBy(e => EF.Property<object>(e, request.SortBy));
        }

        ApplyPagination((request.PageNumber - 1) * request.PageSize, request.PageSize);


        AddIncludes();

        if (asNoTracking)
            ApplyAsNoTracking();

        ApplyOrderByDescending(x => x.Id);
    }

    public DisbursementRequestSpecification(int id)
        : base(x => x.Id == id && !x.IsDeleted)
    {
        AddIncludes();
    }


    private void AddIncludes()
    {
        AddInclude(x => x.Items);
        AddInclude(x => x.JobDepartment);
        AddInclude(x => x.CreatedBy);
        AddInclude(x => x.UpdatedBy);
        AddInclude("Items.Item");
        AddInclude("Items.Item.Unit");
    }
}
