using mogaERP.Domain.Contracts.InventoryModule.Stores;

namespace mogaERP.Domain.Interfaces.InventoryModule;
public interface IStoreService
{
    Task<ApiResponse<string>> CreateStoreAsync(StoreRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateStoreAsync(int id, StoreRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteStoreAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<StoreResponse>> GetStoreByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IReadOnlyList<StoreResponse>>> GetAllStoresAsync(SearchRequest request, CancellationToken cancellationToken = default);
}
