namespace mogaERP.Services.Specifications;
public class BankSpecification : BaseSpecification<Bank>
{
    public BankSpecification(SearchRequest request, bool asNoTracking = true)
        : base(x => !x.IsDeleted)
    {
        // Search by Name, Code, or AccountNumber
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();
            AddCriteria(x =>
                x.Name.Contains(term) ||
                (x.Code != null && x.Code.Contains(term)) ||
                (x.AccountNumber != null && x.AccountNumber.Contains(term))
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

        ApplyPagination((request.PageNumber - 1) * request.PageSize, request.PageSize);

        AddIncludes();

        if (asNoTracking)
            ApplyAsNoTracking();
    }

    public BankSpecification(int id)
        : base(x => x.Id == id && !x.IsDeleted)
    {
        AddIncludes();
    }

    private void AddIncludes()
    {
        AddInclude(x => x.CreatedBy);
        AddInclude(x => x.UpdatedBy);
    }
}