using mogaERP.Domain.Contracts.HR.JobType;

namespace mogaERP.Domain.Interfaces.HR_Module;


public interface IJobTypeService
{
    Task<ApiResponse<string>> CreateAsync(JobTypeRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateAsync(int id, JobTypeRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IReadOnlyList<JobTypeResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default);
    Task<ApiResponse<JobTypeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}