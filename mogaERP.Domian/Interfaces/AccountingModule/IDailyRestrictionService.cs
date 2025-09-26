using mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;

namespace mogaERP.Domain.Interfaces.AccountingModule;
public interface IDailyRestrictionService
{
    Task<string> GenerateRestrictionNumberAsync(CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> CreateAsync(DailyRestrictionRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateAsync(int id, DailyRestrictionRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<DailyRestrictionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<DailyRestrictionResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<AccountReportResponse>>> GetAccountReportAsync(int accountId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default);
}
