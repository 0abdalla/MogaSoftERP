using mogaERP.Domain.Contracts.AccountingModule.FiscalYear;

namespace mogaERP.Domain.Interfaces.AccountingModule;
public interface IFiscalYearService
{
    Task<ApiResponse<string>> CreateAsync(FiscalYearRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateAsync(int id, FiscalYearRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<ApiResponse<FiscalYearResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IReadOnlyList<FiscalYearResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default);
}
