using mogaERP.Domain.Contracts.AccountingModule.AdditionNotice;
using mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;

namespace mogaERP.Domain.Interfaces.AccountingModule;
public interface IAdditionNotificationService
{
    Task<ApiResponse<PartialDailyRestrictionResponse>> CreateAsync(AdditionNotificationRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<AdditionNotificationResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default);
    Task<ApiResponse<AdditionNotificationResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateAsync(int id, AdditionNotificationRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
