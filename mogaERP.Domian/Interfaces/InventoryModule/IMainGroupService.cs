using mogaERP.Domain.Contracts.InventoryModule.Items;

namespace mogaERP.Domain.Interfaces.InventoryModule;
public interface IMainGroupService
{
    Task<ApiResponse<string>> CreateMainGroupAsync(MainGroupRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateMainGroupAsync(int id, MainGroupRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteMainGroupAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<MainGroupResponse>> GetMainGroupByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IReadOnlyList<MainGroupResponse>>> GetAllMainGroupsAsync(SearchRequest request, CancellationToken cancellationToken = default);
}
