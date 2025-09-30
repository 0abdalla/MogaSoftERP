using mogaERP.Domain.Contracts.SalesModule.Customer;

namespace mogaERP.Domain.Interfaces.SalesModule;
public interface ICustomerService
{
    Task<ApiResponse<string>> CreateAsync(CustomerRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<PagedResponse<CustomerResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<CustomerResponse>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ApiResponse<string>> UpdateAsync(int id, CustomerRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken);
}
