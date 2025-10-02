
namespace mogaERP.Services.Specifications;
public class TreasurySpecification : BaseSpecification<Treasury>
{
    public TreasurySpecification(SearchRequest request, bool asNoTracking = true)
        : base(x => !x.IsDeleted)
    {
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();

            AddCriteria(x =>
                (x.Name != null && x.Name.Contains(term))
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
    }
    public TreasurySpecification(int id)
        : base(x => x.Id == id && !x.IsDeleted)
    {
        AddIncludes();
    }

    private void AddIncludes()
    {
        Includes.Add(x => x.Branch);
        Includes.Add(x => x.UpdatedBy);
        Includes.Add(x => x.UpdatedBy);
    }

}
