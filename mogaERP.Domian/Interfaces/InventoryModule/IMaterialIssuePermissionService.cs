using mogaERP.Domain.Contracts.InventoryModule.MaterialIssues;

namespace mogaERP.Domain.Interfaces.InventoryModule;
public interface IMaterialIssuePermissionService
{
    Task<ApiResponse<MaterialIssuePermissionToReturnResponse>> CreateAsync(MaterialIssuePermissionRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateAsync(int id, MaterialIssuePermissionRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<MaterialIssuePermissionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<MaterialIssuePermissionResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken = default);
}