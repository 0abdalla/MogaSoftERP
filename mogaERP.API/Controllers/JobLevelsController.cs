using mogaERP.Domain.Contracts.HR.JobLevel;
using mogaERP.Domain.Interfaces.HR_Module;

namespace mogaERP.API.Controllers;

public class JobLevelsController(IJobLevelService jobLevelService) : BaseApiController
{
    private readonly IJobLevelService _jobLevelService = jobLevelService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] JobLevelRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _jobLevelService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] JobLevelRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _jobLevelService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
    {
        var result = await _jobLevelService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SearchRequest search, CancellationToken cancellationToken = default)
    {
        var result = await _jobLevelService.GetAllAsync(search, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var result = await _jobLevelService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
