using mogaERP.Domain.Contracts.SalesModule.SalesInvoices;
using mogaERP.Domain.Contracts.SalesModule.SalesQuotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mogaERP.Domain.Interfaces.SalesModule
{
    public interface ISalesInvoiceService
    {
        Task<ApiResponse<SalesInvoiceResponse>> CreateAsync(SalesInvoiceRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<SalesInvoiceResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<SalesInvoiceResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<string>> UpdateAsync(int id, SalesInvoiceRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
