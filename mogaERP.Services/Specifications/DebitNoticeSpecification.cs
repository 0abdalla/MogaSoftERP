namespace mogaERP.Services.Specifications;

public class DebitNoticeSpecification : BaseSpecification<DebitNotice>
{
    public DebitNoticeSpecification(SearchRequest request, bool asNoTracking = true)
        : base(x => !x.IsDeleted)
    {
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();
            AddCriteria(x =>
                (x.CheckNumber != null && x.CheckNumber.Contains(term))
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

    public DebitNoticeSpecification(int id)
        : base(x => x.Id == id && !x.IsDeleted)
    {
        AddIncludes();
    }


    private void AddIncludes()
    {
        AddInclude(x => x.Bank);
        AddInclude(x => x.Account);
        AddInclude(x => x.DailyRestriction);
        AddInclude(x => x.DailyRestriction.AccountingGuidance);
        AddInclude(x => x.CreatedBy);
        AddInclude(x => x.UpdatedBy);
    }
}