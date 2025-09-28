using mogaERP.Domain.Contracts.HR.Staff;

namespace mogaERP.Domain.Interfaces.HR_Module;
public interface IStaffService
{
    Task<ApiResponse<string>> CreateAsync(StaffRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<StaffResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<StaffListResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateAsync(int id, StaffRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<ApiResponse<string>> InActiveStaffAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<StaffCountsResponse>> GetStaffCountsAsync(CancellationToken cancellationToken = default);

    //Task<PagedResponseModel<DataTable>> GetCountsAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
}
