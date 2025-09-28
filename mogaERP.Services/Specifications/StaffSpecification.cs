namespace mogaERP.Services.Specifications;
public class StaffSpecification : BaseSpecification<Staff>
{
    public StaffSpecification(SearchRequest request, bool asNoTracking = true)
        : base(x => !x.IsDeleted)
    {
        // Search by Name, Code, Email, Phone, or NationalId
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            AddCriteria(x =>
                x.FullName.ToLower().Contains(term) ||
                (x.Code != null && x.Code.ToLower().Contains(term)) ||
                x.Email.ToLower().Contains(term) ||
                x.PhoneNumber.Contains(term) ||
                (x.NationalId != null && x.NationalId.Contains(term))
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

    public StaffSpecification(int id)
        : base(x => x.Id == id && !x.IsDeleted)
    {
        AddIncludes();
    }

    private void AddIncludes()
    {
        AddInclude(x => x.JobTitle);
        AddInclude(x => x.JobType);
        AddInclude(x => x.JobLevel);
        AddInclude(x => x.JobDepartment);
        AddInclude(x => x.Branch);
        AddInclude(x => x.StaffAttachments);
        AddInclude(x => x.CreatedBy);
        AddInclude(x => x.UpdatedBy);
    }
}