using mogaERP.Domain.Contracts.HR.JobTitle;

namespace mogaERP.Domain.Interfaces.HR_Module;
public interface IJobTitleService
{
    Task<ApiResponse<string>> CreateAsync(JobTitleRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateAsync(int id, JobTitleRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IReadOnlyList<JobTitleResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default);
    Task<ApiResponse<JobTitleResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}