
namespace mogaERP.Services.Specifications;
public class MaterialIssuePermissionSpecification : BaseSpecification<MaterialIssuePermission>
{
    public MaterialIssuePermissionSpecification(SearchRequest request, bool asNoTracking = true)
        : base(x => !x.IsDeleted)
    {
        AddIncludes();

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            var search = request.SearchTerm.Trim().ToLower();
            AddCriteria(x => x.PermissionNumber.ToLower().Contains(search) ||
                             (x.DocumentNumber != null && x.DocumentNumber.ToLower().Contains(search)));

        }

        ApplyOrderByDescending(x => x.Id);
        ApplyPagination((request.PageNumber - 1) * request.PageSize, request.PageSize);


        if (asNoTracking)
            ApplyAsNoTracking();

    }

    public MaterialIssuePermissionSpecification(int id)
        : base(x => x.Id == id && !x.IsDeleted)
    {
        AddIncludes();
    }

    private void AddIncludes()
    {
        AddInclude(x => x.Items);
        AddInclude(x => x.Store);
        AddInclude(x => x.JobDepartment);

        AddInclude(x => x.DisbursementRequest);
        AddInclude(x => x.DailyRestriction);
        AddInclude("Items.Item");

        AddInclude(x => x.CreatedBy);
        AddInclude(x => x.UpdatedBy);
    }
}
