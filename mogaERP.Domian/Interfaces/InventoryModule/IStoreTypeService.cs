using mogaERP.Domain.Contracts.InventoryModule.Stores;

namespace mogaERP.Domain.Interfaces.InventoryModule;
public interface IStoreTypeService
{
    Task<ApiResponse<string>> CreateAsync(StoreTypeRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateAsync(int id, StoreTypeRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<StoreTypeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IReadOnlyList<StoreTypeResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken = default);
}
