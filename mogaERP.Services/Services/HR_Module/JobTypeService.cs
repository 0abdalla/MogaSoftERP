using Microsoft.Extensions.Logging;
using mogaERP.Domain.Contracts.HR.JobType;
using mogaERP.Domain.Interfaces.HR_Module;

namespace mogaERP.Services.Services.HR_Module;
public class JobTypeService(IUnitOfWork unitOfWork, ILogger<JobTypeService> logger) : IJobTypeService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<JobTypeService> _logger = logger;

    public async Task<ApiResponse<string>> CreateAsync(JobTypeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return ApiResponse<string>.Failure(new ErrorModel("Name is required.", AppStatusCode.Failed));

            var exists = await _unitOfWork.Repository<JobType>()
                .AnyAsync(x => x.Name == request.Name && !x.IsDeleted, cancellationToken);

            if (exists)
                return ApiResponse<string>.Failure(new ErrorModel("Job type with this name already exists.", AppStatusCode.Conflict));

            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var status))
                return ApiResponse<string>.Failure(AppErrors.InvalidStatus);

            var entity = new JobType
            {
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                Status = status
            };

            await _unitOfWork.Repository<JobType>().AddAsync(entity, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(ErrorModel.None, entity.Id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating JobType");
            return ApiResponse<string>.Failure(new ErrorModel("Failed to create job type.", AppStatusCode.Failed));
        }
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, JobTypeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<JobType>().GetByIdAsync(id, cancellationToken);
            if (entity == null || entity.IsDeleted)
                return ApiResponse<string>.Failure(new ErrorModel("Job type not found.", AppStatusCode.NotFound));

            if (string.IsNullOrWhiteSpace(request.Name))
                return ApiResponse<string>.Failure(new ErrorModel("Name is required.", AppStatusCode.Failed));

            var exists = await _unitOfWork.Repository<JobType>()
                .AnyAsync(x => x.Name == request.Name && x.Id != id && !x.IsDeleted, cancellationToken);

            if (exists)
                return ApiResponse<string>.Failure(new ErrorModel("Job type with this name already exists.", AppStatusCode.Conflict));

            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var status))
                return ApiResponse<string>.Failure(AppErrors.InvalidStatus);

            entity.Name = request.Name.Trim();
            entity.Description = request.Description?.Trim();
            entity.Status = status;

            _unitOfWork.Repository<JobType>().Update(entity);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(ErrorModel.None, entity.Id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating JobType {Id}", id);
            return ApiResponse<string>.Failure(new ErrorModel("Failed to update job type.", AppStatusCode.Failed));
        }
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<JobType>().GetByIdAsync(id, cancellationToken);
            if (entity == null || entity.IsDeleted)
                return ApiResponse<string>.Failure(new ErrorModel("Job type not found.", AppStatusCode.NotFound));

            entity.IsDeleted = true;
            _unitOfWork.Repository<JobType>().Update(entity);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(ErrorModel.None, entity.Id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting JobType {Id}", id);
            return ApiResponse<string>.Failure(new ErrorModel("Failed to delete job type.", AppStatusCode.Failed));
        }
    }

    public async Task<ApiResponse<IReadOnlyList<JobTypeResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<JobType>().Query(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchRequest.SearchTerm))
            {
                var term = searchRequest.SearchTerm.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(term));
            }

            var jobTypes = await query
                .OrderByDescending(x => x.Id)
                .Select(x => new JobTypeResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Status = x.Status.ToString(),
                    CreatedById = x.CreatedById,
                })
                .ToListAsync(cancellationToken);

            return ApiResponse<IReadOnlyList<JobTypeResponse>>.Success(ErrorModel.None, jobTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all JobTypes");
            return ApiResponse<IReadOnlyList<JobTypeResponse>>.Failure(new ErrorModel("Failed to get job types.", AppStatusCode.Failed));
        }
    }

    public async Task<ApiResponse<JobTypeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<JobType>()
                .Query(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Select(x => new JobTypeResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Status = x.Status.ToString(),
                    CreatedById = x.CreatedById,
                    CreatedBy = x.CreatedBy.UserName,
                    CreatedOn = x.CreatedOn,
                    UpdatedById = x.UpdatedById,
                    UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.UserName : null,
                    UpdatedOn = x.UpdatedOn
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
                return ApiResponse<JobTypeResponse>.Failure(new ErrorModel("Job type not found.", AppStatusCode.NotFound));

            return ApiResponse<JobTypeResponse>.Success(ErrorModel.None, entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting JobType {Id}", id);
            return ApiResponse<JobTypeResponse>.Failure(new ErrorModel("Failed to get job type.", AppStatusCode.Failed));
        }
    }
}
