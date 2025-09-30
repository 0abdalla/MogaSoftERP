using Microsoft.Extensions.Logging;
using mogaERP.Domain.Contracts.HR.JobTitle;
using mogaERP.Domain.Interfaces.HR_Module;

namespace mogaERP.Services.Services.HR_Module;
public class JobTitleService(IUnitOfWork unitOfWork, ILogger<JobTitleService> logger) : IJobTitleService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<JobTitleService> _logger = logger;

    public async Task<ApiResponse<string>> CreateAsync(JobTitleRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return ApiResponse<string>.Failure(new ErrorModel("Name is required.", AppStatusCode.Failed));

            var exists = await _unitOfWork.Repository<JobTitle>()
                .AnyAsync(x => x.Name == request.Name && !x.IsDeleted, cancellationToken);

            if (exists)
                return ApiResponse<string>.Failure(new ErrorModel("Job title with this name already exists.", AppStatusCode.Conflict));

            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var status))
                return ApiResponse<string>.Failure(AppErrors.InvalidStatus);

            var entity = new JobTitle
            {
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                Status = status,
                JobDepartmentId = request.JobDepartmentId
            };

            await _unitOfWork.Repository<JobTitle>().AddAsync(entity, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(ErrorModel.None, entity.Id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating JobTitle");
            return ApiResponse<string>.Failure(new ErrorModel("Failed to create job title.", AppStatusCode.Failed));
        }
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, JobTitleRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<JobTitle>().GetByIdAsync(id, cancellationToken);
            if (entity == null || entity.IsDeleted)
                return ApiResponse<string>.Failure(new ErrorModel("Job title not found.", AppStatusCode.NotFound));

            if (string.IsNullOrWhiteSpace(request.Name))
                return ApiResponse<string>.Failure(new ErrorModel("Name is required.", AppStatusCode.Failed));

            var exists = await _unitOfWork.Repository<JobTitle>()
                .AnyAsync(x => x.Name == request.Name && x.Id != id && !x.IsDeleted, cancellationToken);

            if (exists)
                return ApiResponse<string>.Failure(new ErrorModel("Job title with this name already exists.", AppStatusCode.Conflict));

            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var status))
                return ApiResponse<string>.Failure(AppErrors.InvalidStatus);

            entity.Name = request.Name.Trim();
            entity.Description = request.Description?.Trim();
            entity.Status = status;
            entity.JobDepartmentId = request.JobDepartmentId;

            _unitOfWork.Repository<JobTitle>().Update(entity);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(ErrorModel.None, entity.Id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating JobTitle {Id}", id);
            return ApiResponse<string>.Failure(new ErrorModel("Failed to update job title.", AppStatusCode.Failed));
        }
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<JobTitle>().GetByIdAsync(id, cancellationToken);
            if (entity == null || entity.IsDeleted)
                return ApiResponse<string>.Failure(new ErrorModel("Job title not found.", AppStatusCode.NotFound));

            entity.IsDeleted = true;
            _unitOfWork.Repository<JobTitle>().Update(entity);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(ErrorModel.None, entity.Id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting JobTitle {Id}", id);
            return ApiResponse<string>.Failure(new ErrorModel("Failed to delete job title.", AppStatusCode.Failed));
        }
    }

    public async Task<ApiResponse<IReadOnlyList<JobTitleResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<JobTitle>().Query(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchRequest.SearchTerm))
            {
                var term = searchRequest.SearchTerm.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(term));
            }

            var jobTitles = await query
                .OrderByDescending(x => x.Id)
                .Select(x => new JobTitleResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Status = x.Status.ToString(),
                    JobDepartmentId = x.JobDepartmentId,
                    JobDepartmentName = x.JobDepartment != null ? x.JobDepartment.Name : null,
                    CreatedById = x.CreatedById,
                    CreatedOn = x.CreatedOn,
                })
                .ToListAsync(cancellationToken);

            return ApiResponse<IReadOnlyList<JobTitleResponse>>.Success(ErrorModel.None, jobTitles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all JobTitles");
            return ApiResponse<IReadOnlyList<JobTitleResponse>>.Failure(new ErrorModel("Failed to get job titles.", AppStatusCode.Failed));
        }
    }

    public async Task<ApiResponse<JobTitleResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<JobTitle>()
                .Query(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.JobDepartment)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Select(x => new JobTitleResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Status = x.Status.ToString(),
                    JobDepartmentId = x.JobDepartmentId,
                    JobDepartmentName = x.JobDepartment != null ? x.JobDepartment.Name : null,
                    CreatedById = x.CreatedById,
                    CreatedBy = x.CreatedBy.UserName,
                    CreatedOn = x.CreatedOn,
                    UpdatedById = x.UpdatedById,
                    UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.UserName : null,
                    UpdatedOn = x.UpdatedOn
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
                return ApiResponse<JobTitleResponse>.Failure(new ErrorModel("Job title not found.", AppStatusCode.NotFound));

            return ApiResponse<JobTitleResponse>.Success(ErrorModel.None, entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting JobTitle {Id}", id);
            return ApiResponse<JobTitleResponse>.Failure(new ErrorModel("Failed to get job title.", AppStatusCode.Failed));
        }
    }
}