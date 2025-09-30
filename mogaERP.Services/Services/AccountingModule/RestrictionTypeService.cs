using mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.Services.Services.AccountingModule;
public class RestrictionTypeService(IUnitOfWork unitOfWork) : IRestrictionTypeService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ApiResponse<string>> CreateAsync(RestrictionTypeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return ApiResponse<string>.Failure(new ErrorModel("Name is required.", AppStatusCode.Failed));

            var exists = await _unitOfWork.Repository<RestrictionType>()
                .AnyAsync(x => x.Name == request.Name && !x.IsDeleted, cancellationToken);

            if (exists)
                return ApiResponse<string>.Failure(new ErrorModel("Restriction type with this name already exists.", AppStatusCode.Conflict));

            var entity = new RestrictionType
            {
                Name = request.Name.Trim(),
                Description = request.Description?.Trim()
            };

            await _unitOfWork.Repository<RestrictionType>().AddAsync(entity, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess, entity.Id.ToString());
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
            var entity = await _unitOfWork.Repository<RestrictionType>().GetByIdAsync(id, cancellationToken);
            if (entity == null || entity.IsDeleted)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            entity.IsDeleted = true;
            _unitOfWork.Repository<RestrictionType>().Update(entity);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<List<RestrictionTypeResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
    {
        var query = _unitOfWork.Repository<RestrictionType>().Query(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchRequest.SearchTerm))
        {
            var term = searchRequest.SearchTerm.Trim().ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(term));
        }

        var list = await query
            .Select(x => new RestrictionTypeResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<RestrictionTypeResponse>>.Success(AppErrors.Success, list);
    }

    public async Task<ApiResponse<RestrictionTypeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Repository<RestrictionType>()
            .Query(x => x.Id == id && !x.IsDeleted)
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
            return ApiResponse<RestrictionTypeResponse>.Failure(AppErrors.NotFound);

        var response = new RestrictionTypeResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            CreatedById = entity.CreatedById,
            CreatedBy = entity.CreatedBy.UserName,
            CreatedOn = entity.CreatedOn,
            UpdatedById = entity.UpdatedById,
            UpdatedBy = entity.UpdatedBy?.UserName,
            UpdatedOn = entity.UpdatedOn
        };

        return ApiResponse<RestrictionTypeResponse>.Success(AppErrors.Success, response);
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, RestrictionTypeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<RestrictionType>().GetByIdAsync(id, cancellationToken);
            if (entity == null || entity.IsDeleted)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            if (string.IsNullOrWhiteSpace(request.Name))
                return ApiResponse<string>.Failure(new ErrorModel("Name is required.", AppStatusCode.Failed));

            var exists = await _unitOfWork.Repository<RestrictionType>()
                .AnyAsync(x => x.Name == request.Name && x.Id != id && !x.IsDeleted, cancellationToken);

            if (exists)
                return ApiResponse<string>.Failure(new ErrorModel("Restriction type with this name already exists.", AppStatusCode.Conflict));

            entity.Name = request.Name.Trim();
            entity.Description = request.Description?.Trim();

            _unitOfWork.Repository<RestrictionType>().Update(entity);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.UpdateSuccess, entity.Id.ToString());
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }
}
