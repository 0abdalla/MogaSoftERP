using mogaERP.Domain.Contracts.ProcurementModule.PurchaseOrder;

namespace mogaERP.Domain.Interfaces.ProcurementModule;
public interface IPurchaseOrderService
{
    Task<ApiResponse<string>> CreateAsync(PurchaseOrderRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateAsync(int id, PurchaseOrderRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PurchaseOrderResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PurchaseOrderResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
