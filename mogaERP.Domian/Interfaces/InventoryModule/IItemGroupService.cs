using mogaERP.Domain.Contracts.InventoryModule.Items;

namespace mogaERP.Domain.Interfaces.InventoryModule;
public interface IItemGroupService
{
    Task<ApiResponse<string>> CreateItemGroupAsync(ItemGroupRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateItemGroupAsync(int id, ItemGroupRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteItemGroupAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<ItemGroupResponse>> GetItemGroupByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IReadOnlyList<ItemGroupResponse>>> GetAllItemGroupsAsync(SearchRequest request, CancellationToken cancellationToken = default);
}
