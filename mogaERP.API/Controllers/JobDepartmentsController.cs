using mogaERP.Domain.Contracts.HR.JobDepartment;
using mogaERP.Domain.Interfaces.HR_Module;

namespace mogaERP.API.Controllers;
public class JobDepartmentsController(IJobDepartmentService jobDepartmentService) : BaseApiController
{
    private readonly IJobDepartmentService _jobDepartmentService = jobDepartmentService;

    [HttpPost]
    public async Task<IActionResult> Create(JobDepartmentRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _jobDepartmentService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, JobDepartmentRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _jobDepartmentService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
    {
        var result = await _jobDepartmentService.GetAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SearchRequest search, CancellationToken cancellationToken = default)
    {
        var result = await _jobDepartmentService.GetAllAsync(search, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var result = await _jobDepartmentService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
