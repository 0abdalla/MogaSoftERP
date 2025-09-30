using mogaERP.Domain.Contracts.SalesModule.SalesQuotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mogaERP.Domain.Interfaces.SalesModule
{
    public interface ISalesQuotationService
    {
        Task<ApiResponse<string>> CreateAsync(SalesQuotationRequest request, CancellationToken cancellationToken);
        Task<ApiResponse<List<SalesQuotationResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken);
        Task<ApiResponse<SalesQuotationResponse>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<ApiResponse<string>> UpdateAsync(int id, SalesQuotationRequest request, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
