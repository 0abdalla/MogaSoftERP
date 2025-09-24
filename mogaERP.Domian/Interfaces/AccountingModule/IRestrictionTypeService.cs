using mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;

namespace mogaERP.Domain.Interfaces.AccountingModule;
public interface IRestrictionTypeService
{
    Task<ApiResponse<string>> CreateAsync(RestrictionTypeRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateAsync(int id, RestrictionTypeRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<RestrictionTypeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<RestrictionTypeResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default);
}
