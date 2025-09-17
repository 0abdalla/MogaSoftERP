using mogaERP.Domain.Contracts.InventoryModule.Items;

namespace mogaERP.Domain.Interfaces.InventoryModule;
public interface IItemUnitService
{
    Task<ApiResponse<string>> CreateItemUnitAsync(ItemUnitRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateItemUnitAsync(int id, ItemUnitRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteItemUnitAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<ItemUnitResponse>> GetItemUnitByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IReadOnlyList<ItemUnitResponse>>> GetAllItemUnitsAsync(SearchRequest request, CancellationToken cancellationToken = default);
}
