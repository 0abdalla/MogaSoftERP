using mogaERP.Domain.Contracts.AccountingModule.Bank;

namespace mogaERP.Domain.Interfaces.AccountingModule;
public interface IBankService
{
    Task<ApiResponse<string>> CreateAsync(BankRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateAsync(int id, BankRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<BankResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<BankResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
