using mogaERP.Domain.Contracts.InventoryModule.Items;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.Services.Services.InventoryModule;
public class ItemUnitService(IUnitOfWork unitOfWork) : IItemUnitService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ApiResponse<string>> CreateItemUnitAsync(ItemUnitRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var unit = new ItemUnit
            {
                Name = request.Name
            };

            await _unitOfWork.Repository<ItemUnit>().AddAsync(unit, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess, unit.Id.ToString());
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> DeleteItemUnitAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var unit = await _unitOfWork.Repository<ItemUnit>().GetByIdAsync(id, cancellationToken);
            if (unit == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            unit.IsDeleted = true;
            _unitOfWork.Repository<ItemUnit>().Update(unit);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<IReadOnlyList<ItemUnitResponse>>> GetAllItemUnitsAsync(SearchRequest request, CancellationToken cancellationToken = default)
    {
        var query = _unitOfWork.Repository<ItemUnit>()
         .Query(u => !u.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.ToLower();
            query = query.Where(u => u.Name.ToLower().Contains(search));
        }

        var units = await query
            .Select(u => new ItemUnitResponse
            {
                Id = u.Id,
                Name = u.Name
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<IReadOnlyList<ItemUnitResponse>>.Success(AppErrors.Success, units);
    }

    public async Task<ApiResponse<ItemUnitResponse>> GetItemUnitByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var unit = await _unitOfWork.Repository<ItemUnit>()
                .Query(u => u.Id == id && !u.IsDeleted)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(cancellationToken);

            if (unit == null)
                return ApiResponse<ItemUnitResponse>.Failure(AppErrors.NotFound);

            var unitResponse = new ItemUnitResponse
            {
                Id = unit.Id,
                Name = unit.Name,
                CreatedBy = unit.CreatedBy.UserName ?? string.Empty,
                CreatedById = unit.CreatedById,
                CreatedOn = unit.CreatedOn,
                UpdatedBy = unit?.UpdatedBy?.UserName ?? string.Empty,
                UpdatedById = unit?.UpdatedById,
                UpdatedOn = unit?.UpdatedOn
            };

            return ApiResponse<ItemUnitResponse>.Success(AppErrors.Success, unitResponse);
        }
        catch (Exception)
        {
            return ApiResponse<ItemUnitResponse>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> UpdateItemUnitAsync(int id, ItemUnitRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var unit = await _unitOfWork.Repository<ItemUnit>().GetByIdAsync(id, cancellationToken);
            if (unit == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);
            unit.Name = request.Name;
            _unitOfWork.Repository<ItemUnit>().Update(unit);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }
}
