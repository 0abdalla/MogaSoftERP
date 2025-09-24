namespace mogaERP.Services.Specifications;

public class DailyRestrictionSpecification : BaseSpecification<DailyRestriction>
{
    public DailyRestrictionSpecification(SearchRequest request, bool asNoTracking = true)
        : base(x => !x.IsDeleted)
    {
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();
            AddCriteria(x =>
                (x.RestrictionNumber != null && x.RestrictionNumber.Contains(term))

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
            ApplyOrderByDescending(x => x.RestrictionDate);
        }

        // Pagination
        ApplyPagination((request.PageNumber - 1) * request.PageSize, request.PageSize);

        AddIncludes();

        if (asNoTracking)
            ApplyAsNoTracking();

    }

    public DailyRestrictionSpecification(int id)
        : base(x => x.Id == id && !x.IsDeleted)
    {
        AddIncludes();
    }

    private void AddIncludes()
    {
        AddInclude(x => x.RestrictionType);
        AddInclude(x => x.AccountingGuidance);
        AddInclude(x => x.Details);
        AddInclude("Details.Account");
        AddInclude("Details.CostCenter");
        AddInclude(x => x.CreatedBy);
        AddInclude(x => x.UpdatedBy);
    }
}