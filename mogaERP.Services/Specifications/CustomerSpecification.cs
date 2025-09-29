namespace mogaERP.Services.Specifications;
public class CustomerSpecification : BaseSpecification<Customer>
{
    public CustomerSpecification(SearchRequest request, bool asNoTracking = true)
        : base(x => !x.IsDeleted)
    {
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();

            AddCriteria(x =>
                (x.Name != null && x.Name.Contains(term) ||
                (x.AccountCode != null && x.AccountCode.Contains(term))) ||
                (x.PhoneNumber != null && x.PhoneNumber.Contains(term))
            );
        }

        // Sorting
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

        // Pagination
        ApplyPagination((request.PageNumber - 1) * request.PageSize, request.PageSize);

        AddIncludes();

        if (asNoTracking)
            ApplyAsNoTracking();
    }

    public CustomerSpecification(int id)
        : base(x => x.Id == id && !x.IsDeleted)
    {
        AddIncludes();
    }

    private void AddIncludes()
    {
        Includes.Add(x => x.CreatedBy);
        Includes.Add(x => x.UpdatedBy);
    }
}
