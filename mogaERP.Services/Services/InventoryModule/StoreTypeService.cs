using mogaERP.Domain.Contracts.InventoryModule.Stores;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.Services.Services.InventoryModule;
public class StoreTypeService(IUnitOfWork unitOfWork) : IStoreTypeService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ApiResponse<string>> CreateAsync(StoreTypeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var storeType = new StoreType
            {
                Name = request.Name,
            };

            await _unitOfWork.Repository<StoreType>().AddAsync(storeType, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess, storeType.Id.ToString());

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
            var storeType = await _unitOfWork.Repository<StoreType>().GetByIdAsync(id, cancellationToken);
            if (storeType == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);
            storeType.IsDeleted = true;
            _unitOfWork.Repository<StoreType>().Update(storeType);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<IReadOnlyList<StoreTypeResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken = default)
    {
        var query = _unitOfWork.Repository<StoreType>().Query(st => !st.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(st => st.Name.ToLower().Contains(request.SearchTerm.ToLower()));
        }

        var storeTypes = await query
            .Select(st => new StoreTypeResponse
            {
                Id = st.Id,
                Name = st.Name,
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<IReadOnlyList<StoreTypeResponse>>.Success(AppErrors.Success, storeTypes);

    }

    public async Task<ApiResponse<StoreTypeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var storeType = await _unitOfWork.Repository<StoreType>()
            .Query(x => x.Id == id && !x.IsDeleted)
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .FirstOrDefaultAsync(cancellationToken);

        if (storeType == null || storeType.IsDeleted)
            return ApiResponse<StoreTypeResponse>.Failure(AppErrors.NotFound);


        var storeTypeResponse = new StoreTypeResponse
        {
            Id = storeType.Id,
            Name = storeType.Name,
            CreatedBy = storeType.CreatedBy.UserName,
            CreatedById = storeType.CreatedById,
            CreatedOn = storeType.CreatedOn,
            UpdatedBy = storeType.UpdatedBy != null ? storeType.UpdatedBy.UserName : string.Empty,
            UpdatedById = storeType.UpdatedById,
            UpdatedOn = storeType.UpdatedOn
        };

        return ApiResponse<StoreTypeResponse>.Success(AppErrors.Success, storeTypeResponse);
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, StoreTypeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var storeType = await _unitOfWork.Repository<StoreType>().GetByIdAsync(id, cancellationToken);
            if (storeType == null || storeType.IsDeleted)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            storeType.Name = request.Name;

            _unitOfWork.Repository<StoreType>().Update(storeType);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }
}
