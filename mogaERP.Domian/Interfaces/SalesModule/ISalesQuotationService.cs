using mogaERP.Domain.Contracts.SalesModule.SalesQuotation;

namespace mogaERP.Domain.Interfaces.SalesModule
{
    public interface ISalesQuotationService
    {
        Task<ApiResponse<string>> CreateAsync(SalesQuotationRequest request, CancellationToken cancellationToken);
        Task<ApiResponse<PagedResponse<SalesQuotationResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken);
        Task<ApiResponse<SalesQuotationResponse>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<ApiResponse<string>> UpdateAsync(int id, SalesQuotationRequest request, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
