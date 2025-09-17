using mogaERP.Domain.Contracts.InventoryModule.Items;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.Services.Services.InventoryModule;
public class ItemGroupService(IUnitOfWork unitOfWork) : IItemGroupService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ApiResponse<string>> CreateItemGroupAsync(ItemGroupRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var itemGroup = new ItemGroup
            {
                Name = request.Name,
                MainGroupId = request.MainGroupId
            };

            await _unitOfWork.Repository<ItemGroup>().AddAsync(itemGroup, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess, itemGroup.Id.ToString());
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> DeleteItemGroupAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var itemGroup = await _unitOfWork.Repository<ItemGroup>().GetByIdAsync(id, cancellationToken);
            if (itemGroup == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);
            itemGroup.IsDeleted = true;
            _unitOfWork.Repository<Domain.Entities.ItemGroup>().Update(itemGroup);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<IReadOnlyList<ItemGroupResponse>>> GetAllItemGroupsAsync(SearchRequest request, CancellationToken cancellationToken = default)
    {
        var query = _unitOfWork.Repository<ItemGroup>()
        .Query(ig => !ig.IsDeleted)
        .Include(ig => ig.MainGroup)
        .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.ToLower();
            query = query.Where(ig =>
                ig.Name.ToLower().Contains(search));
        }

        var itemGroups = await query
            .Select(ig => new ItemGroupResponse
            {
                Id = ig.Id,
                Name = ig.Name,
                MainGroupId = ig.MainGroupId,
                MainGroupName = ig.MainGroup != null ? ig.MainGroup.Name : null
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<IReadOnlyList<ItemGroupResponse>>.Success(AppErrors.Success, itemGroups);

    }

    public async Task<ApiResponse<ItemGroupResponse>> GetItemGroupByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var itemGroup = await _unitOfWork.Repository<ItemGroup>()
            .Query(ig => ig.Id == id && !ig.IsDeleted)
            .Include(ig => ig.CreatedBy)
            .Include(ig => ig.UpdatedBy)
            .Include(ig => ig.MainGroup)
            .Select(ig => new ItemGroupResponse
            {
                Id = ig.Id,
                Name = ig.Name,
                MainGroupId = ig.MainGroupId,
                MainGroupName = ig.MainGroup != null ? ig.MainGroup.Name : null
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (itemGroup == null)
            return ApiResponse<ItemGroupResponse>.Failure(AppErrors.NotFound);

        return ApiResponse<ItemGroupResponse>.Success(AppErrors.Success, itemGroup);
    }

    public async Task<ApiResponse<string>> UpdateItemGroupAsync(int id, ItemGroupRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var itemGroup = await _unitOfWork.Repository<ItemGroup>().GetByIdAsync(id, cancellationToken);
            if (itemGroup == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);
            itemGroup.Name = request.Name;
            _unitOfWork.Repository<ItemGroup>().Update(itemGroup);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }
}
