using mogaERP.Domain.Contracts.HR.JobDepartment;

namespace mogaERP.Domain.Interfaces.HR_Module;
public interface IJobDepartmentService
{
    Task<ApiResponse<string>> CreateAsync(JobDepartmentRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> UpdateAsync(int id, JobDepartmentRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<JobDepartmentResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IReadOnlyList<JobDepartmentResponse>>> GetAllAsync(SearchRequest search, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
