using mogaERP.Domain.Contracts.InventoryModule.DisbursementRequest;

namespace mogaERP.Domain.Interfaces.InventoryModule;
public interface IDisbursementRequestService
{
    Task<ApiResponse<DisbursementToReturnResponse>> CreateAsync(DisbursementRequestRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateAsync(int id, DisbursementRequestRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<DisbursementRequestResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<DisbursementRequestResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> ApproveDisbursementRequestAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<DisbursementRequestResponse>>> GetAllApprovedAsync(SearchRequest request, CancellationToken cancellationToken = default);
}
