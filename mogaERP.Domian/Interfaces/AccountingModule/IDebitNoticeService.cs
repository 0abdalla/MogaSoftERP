using mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;
using mogaERP.Domain.Contracts.AccountingModule.DebitNotice;

namespace mogaERP.Domain.Interfaces.AccountingModule;
public interface IDebitNoticeService
{
    Task<ApiResponse<PartialDailyRestrictionResponse>> CreateAsync(DebitNoticeRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<DebitNoticeResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default);
    Task<ApiResponse<DebitNoticeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateAsync(int id, DebitNoticeRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
