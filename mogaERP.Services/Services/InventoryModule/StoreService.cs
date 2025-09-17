using mogaERP.Domain.Contracts.InventoryModule.Stores;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.Services.Services.InventoryModule;
public class StoreService(IUnitOfWork unitOfWork) : IStoreService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ApiResponse<string>> CreateStoreAsync(StoreRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var store = new Store
            {
                Name = request.Name,
                Address = request.Address,
                Code = request.Code,
                PhoneNumber = request.PhoneNumber,

                StoreTypeId = request.StoreTypeId,
            };

            await _unitOfWork.Repository<Store>().AddAsync(store, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<string>> DeleteStoreAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var store = await _unitOfWork.Repository<Store>().GetByIdAsync(id, cancellationToken);

            if (store == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);


            store.IsDeleted = true;
            _unitOfWork.Repository<Store>().Update(store);

            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<IReadOnlyList<StoreResponse>>> GetAllStoresAsync(SearchRequest request, CancellationToken cancellationToken = default)
    {
        var query = _unitOfWork.Repository<Store>()
         .Query(s => !s.IsDeleted)
         .Include(s => s.StoreType)
         .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.ToLower();

            query = query.Where(s =>
                s.Name.ToLower().Contains(search) ||
                s.Code.ToLower().Contains(search));
        }

        var stores = await query
            .Select(s => new StoreResponse
            {
                Id = s.Id,
                Name = s.Name,
                Address = s.Address,
                Code = s.Code,
                PhoneNumber = s.PhoneNumber,
                StoreTypeId = s.StoreTypeId,
                StoreTypeName = s.StoreType != null ? s.StoreType.Name : string.Empty,
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<IReadOnlyList<StoreResponse>>.Success(AppErrors.Success, stores);
    }

    public async Task<ApiResponse<StoreResponse>> GetStoreByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var store = await _unitOfWork.Repository<Store>()
                        .Query(s => s.Id == id && !s.IsDeleted)
                        .Include(s => s.CreatedBy)
                        .Include(s => s.UpdatedBy)
                        .Include(s => s.StoreType)
                        .FirstOrDefaultAsync(cancellationToken);

        if (store == null)
        {
            return ApiResponse<StoreResponse>.Failure(AppErrors.NotFound);
        }

        var response = new StoreResponse
        {
            Id = store.Id,
            Name = store.Name,
            Address = store.Address,
            Code = store.Code,
            PhoneNumber = store.PhoneNumber,
            StoreTypeName = store.StoreType != null ? store.StoreType.Name : string.Empty,
            StoreTypeId = store.StoreTypeId,
            // Audit 
            CreatedBy = store?.CreatedBy?.UserName ?? string.Empty,
            CreatedOn = store.CreatedOn,
            CreatedById = store.CreatedById,

            UpdatedBy = store?.UpdatedBy?.UserName ?? string.Empty,
            UpdatedById = store.UpdatedById,
            UpdatedOn = store.UpdatedOn,
        };

        return ApiResponse<StoreResponse>.Success(AppErrors.Success, response);
    }

    public async Task<ApiResponse<string>> UpdateStoreAsync(int id, StoreRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var store = await _unitOfWork.Repository<Store>().GetByIdAsync(id, cancellationToken);

            if (store == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            store.Name = request.Name;
            store.Address = request.Address;
            store.Code = request.Code;
            store.PhoneNumber = request.PhoneNumber;
            store.StoreTypeId = request.StoreTypeId;

            _unitOfWork.Repository<Store>().Update(store);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }
}
