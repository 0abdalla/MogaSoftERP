using mogaERP.Domain.Contracts.AccountingModule.Treasury;

namespace mogaERP.Domain.Interfaces.AccountingModule;
public interface ITreasuryService
{
    Task<ApiResponse<string>> CreateTreasuryAsync(TreasuryRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateTreasuryAsync(int id, TreasuryRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<TreasuryResponse>> GetTreasuryByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<TreasuryResponse>>> GetTreasuriesAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteTreasuryAsync(int id, CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> EnableTreasuryMovementAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DisableTreasuryMovementAsync(int id, DateOnly closeInDate, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<TreasuryMovementResponse>>> GetAllMovementsAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<List<TreasuryMovementResponse>>> GetEnabledTreasuriesMovementsAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<List<TreasuryMovementResponse>>> GetDisabledTreasuriesMovementsAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<TreasuryMovementResponse>> GetTreasuryMovementByIdAsyncV1(int id, CancellationToken cancellationToken = default);
    // Task<ApiResponse<TreasuryMovementDetailsResponse>> GetTreasuryMovementByIdAsyncV2(int id, CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> ReDisableTreasuryMovementAsync(int id, CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> AssignTreasuryToStaffAsync(int staffId, int treasuryId, CancellationToken cancellationToken);
    Task<ApiResponse<TreasuryTransactionResponse>> GetTreasuryTransactionsAsync(int treasuryId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default);
}
