using mogaERP.Domain.Contracts.HR.JobLevel;

namespace mogaERP.Domain.Interfaces.HR_Module;
public interface IJobLevelService
{
    Task<ApiResponse<string>> CreateAsync(JobLevelRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateAsync(int id, JobLevelRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<JobLevelResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IReadOnlyList<JobLevelResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
