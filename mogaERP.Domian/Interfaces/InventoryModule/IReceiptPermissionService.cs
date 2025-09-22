using mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;
using mogaERP.Domain.Contracts.InventoryModule.ReceiptPermissions;

namespace mogaERP.Domain.Interfaces.InventoryModule;
public interface IReceiptPermissionService
{
    Task<ApiResponse<string>> UpdateAsync(int id, ReceiptPermissionRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<ReceiptPermissionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<ReceiptPermissionResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PartialDailyRestrictionResponse>> CreateAsync(ReceiptPermissionRequest request, CancellationToken cancellationToken = default);
}
