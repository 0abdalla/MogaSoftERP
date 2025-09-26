namespace mogaERP.Services.Specifications;
public class AdditionNotificationSpecification : BaseSpecification<AdditionNotice>
{
    public AdditionNotificationSpecification(SearchRequest searchRequest, bool asNoTracking = true)
        : base(x => !x.IsDeleted)
    {
        AddIncludes();

        if (!string.IsNullOrWhiteSpace(searchRequest.SearchTerm))
        {
            var term = searchRequest.SearchTerm.Trim();
            AddCriteria(x =>
                x.CheckNumber.Contains(term)
            );
        }

        // Sorting
        if (!string.IsNullOrWhiteSpace(searchRequest.SortBy))
        {
            if (searchRequest.SortDescending)
                ApplyOrderByDescending(e => EF.Property<object>(e, searchRequest.SortBy));
            else
                ApplyOrderBy(e => EF.Property<object>(e, searchRequest.SortBy));
        }
        else
        {
            ApplyOrderByDescending(x => x.Id);
        }

        ApplyPagination((searchRequest.PageNumber - 1) * searchRequest.PageSize, searchRequest.PageSize);

        AddIncludes();

        if (asNoTracking)
            ApplyAsNoTracking();
    }

    public AdditionNotificationSpecification(int id)
        : base(x => x.Id == id)
    {
        AddIncludes();
    }

    private void AddIncludes()
    {
        AddInclude(x => x.Account);
        AddInclude(x => x.CreatedBy);
        AddInclude(x => x.UpdatedBy);
        AddInclude(x => x.DailyRestriction);
        AddInclude(x => x.Bank);
    }
}
