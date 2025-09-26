using Microsoft.Extensions.Logging;
using mogaERP.Domain.Contracts.HR.JobLevel;
using mogaERP.Domain.Interfaces.HR_Module;

namespace mogaERP.Services.Services.HR_Module;
public class JobLevelService(IUnitOfWork unitOfWork, ILogger<JobLevelService> logger) : IJobLevelService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<JobLevelService> _logger = logger;

    public async Task<ApiResponse<string>> CreateAsync(JobLevelRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return ApiResponse<string>.Failure(new ErrorModel("Name is required.", AppStatusCode.Failed));

            var exists = await _unitOfWork.Repository<JobLevel>()
                .AnyAsync(x => x.Name == request.Name && !x.IsDeleted, cancellationToken);

            if (exists)
                return ApiResponse<string>.Failure(new ErrorModel("Job level with this name already exists.", AppStatusCode.Conflict));

            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var status))
                return ApiResponse<string>.Failure(AppErrors.InvalidStatus);

            var entity = new JobLevel
            {
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                Status = status
            };

            await _unitOfWork.Repository<JobLevel>().AddAsync(entity, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(ErrorModel.None, entity.Id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating JobLevel");
            return ApiResponse<string>.Failure(new ErrorModel("Failed to create job level.", AppStatusCode.Failed));
        }
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, JobLevelRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<JobLevel>().GetByIdAsync(id, cancellationToken);
            if (entity == null || entity.IsDeleted)
                return ApiResponse<string>.Failure(new ErrorModel("Job level not found.", AppStatusCode.NotFound));

            if (string.IsNullOrWhiteSpace(request.Name))
                return ApiResponse<string>.Failure(new ErrorModel("Name is required.", AppStatusCode.Failed));

            var exists = await _unitOfWork.Repository<JobLevel>()
                .AnyAsync(x => x.Name == request.Name && x.Id != id && !x.IsDeleted, cancellationToken);

            if (exists)
                return ApiResponse<string>.Failure(new ErrorModel("Job level with this name already exists.", AppStatusCode.Conflict));

            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var newStatus))
                return ApiResponse<string>.Failure(AppErrors.InvalidStatus);

            entity.Name = request.Name.Trim();
            entity.Description = request.Description?.Trim();
            entity.Status = newStatus;

            _unitOfWork.Repository<JobLevel>().Update(entity);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(ErrorModel.None, entity.Id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating JobLevel {Id}", id);
            return ApiResponse<string>.Failure(new ErrorModel("Failed to update job level.", AppStatusCode.Failed));
        }
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<JobLevel>().GetByIdAsync(id, cancellationToken);
            if (entity == null || entity.IsDeleted)
                return ApiResponse<string>.Failure(new ErrorModel("Job level not found.", AppStatusCode.NotFound));

            entity.IsDeleted = true;
            _unitOfWork.Repository<JobLevel>().Update(entity);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(ErrorModel.None, entity.Id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting JobLevel {Id}", id);
            return ApiResponse<string>.Failure(new ErrorModel("Failed to delete job level.", AppStatusCode.Failed));
        }
    }

    public async Task<ApiResponse<IReadOnlyList<JobLevelResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<JobLevel>().Query(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchRequest.SearchTerm))
            {
                var term = searchRequest.SearchTerm.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(term));
            }

            var jobLevels = await query
                .OrderByDescending(x => x.Id)
                .Select(x => new JobLevelResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Status = x.Status.ToString(),
                })
                .ToListAsync(cancellationToken);

            return ApiResponse<IReadOnlyList<JobLevelResponse>>.Success(ErrorModel.None, jobLevels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all JobLevels");
            return ApiResponse<IReadOnlyList<JobLevelResponse>>.Failure(new ErrorModel("Failed to get job levels.", AppStatusCode.Failed));
        }
    }

    public async Task<ApiResponse<JobLevelResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<JobLevel>()
                .Query(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Select(x => new JobLevelResponse
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
                return ApiResponse<JobLevelResponse>.Failure(new ErrorModel("Job level not found.", AppStatusCode.NotFound));

            return ApiResponse<JobLevelResponse>.Success(ErrorModel.None, entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting JobLevel {Id}", id);
            return ApiResponse<JobLevelResponse>.Failure(new ErrorModel("Failed to get job level.", AppStatusCode.Failed));
        }
    }
}
