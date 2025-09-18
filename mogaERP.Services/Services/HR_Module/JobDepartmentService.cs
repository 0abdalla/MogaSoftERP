using mogaERP.Domain.Contracts.HR.JobDepartment;
using mogaERP.Domain.Interfaces.HR_Module;

namespace mogaERP.Services.Services.HR_Module;
public class JobDepartmentService(IUnitOfWork unitOfWork) : IJobDepartmentService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ApiResponse<string>> CreateAsync(JobDepartmentRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var isExists = await _unitOfWork.Repository<JobDepartment>().AnyAsync(x => x.Name == request.Name, cancellationToken);

            if (isExists)
                return ApiResponse<string>.Failure(AppErrors.AlreadyExists);


            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var status))
                return ApiResponse<string>.Failure(AppErrors.InvalidStatus);

            var department = new JobDepartment
            {
                Name = request.Name,
                Description = request.Description,
                Status = status,
            };

            await _unitOfWork.Repository<JobDepartment>().AddAsync(department, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess, department.Id.ToString());

        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var department = await _unitOfWork.Repository<JobDepartment>().GetByIdAsync(id, cancellationToken);
            if (department == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            department.IsDeleted = true;

            _unitOfWork.Repository<JobDepartment>().Update(department);

            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<IReadOnlyList<JobDepartmentResponse>>> GetAllAsync(SearchRequest search, CancellationToken cancellationToken = default)
    {
        var query = _unitOfWork.Repository<JobDepartment>().Query(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {
            var searchText = search.SearchTerm.Trim().ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(searchText));
        }

        var totalRecords = await query.CountAsync(cancellationToken);

        var departments = await query
            .OrderByDescending(x => x.Id)
            .Select(x => new JobDepartmentResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Status = x.Status.ToString()
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<IReadOnlyList<JobDepartmentResponse>>.Success(AppErrors.Success, departments);

    }

    public async Task<ApiResponse<JobDepartmentResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var department = await _unitOfWork.Repository<JobDepartment>()
            .Query(x => x.Id == id && !x.IsDeleted)
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .Select(x => new JobDepartmentResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Status = x.Status.ToString(),
                CreatedBy = x.CreatedBy.UserName,
                CreatedOn = x.CreatedOn,
                UpdatedBy = x.UpdatedBy.UserName,
                UpdatedOn = x.UpdatedOn,
                CreatedById = x.CreatedById,
                UpdatedById = x.UpdatedById
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (department == null)
            return ApiResponse<JobDepartmentResponse>.Failure(AppErrors.NotFound);

        return ApiResponse<JobDepartmentResponse>.Success(AppErrors.Success, department);
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, JobDepartmentRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var department = await _unitOfWork.Repository<JobDepartment>().GetByIdAsync(id, cancellationToken);

            if (department == null || department.IsDeleted)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            var isExists = await _unitOfWork.Repository<JobDepartment>().AnyAsync(x => x.Name == request.Name && x.Id != id, cancellationToken);

            if (isExists)
                return ApiResponse<string>.Failure(AppErrors.AlreadyExists);

            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var status))
                return ApiResponse<string>.Failure(AppErrors.InvalidStatus);

            department.Name = request.Name;
            department.Description = request.Description;
            department.Status = status;

            _unitOfWork.Repository<JobDepartment>().Update(department);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }
}
