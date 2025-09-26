using mogaERP.Domain.Contracts.HR.JobType;
using mogaERP.Domain.Interfaces.HR_Module;

namespace mogaERP.API.Controllers;

public class JobTypesController(IJobTypeService jobTypeService) : BaseApiController
{
    private readonly IJobTypeService _jobTypeService = jobTypeService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] JobTypeRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _jobTypeService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] JobTypeRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _jobTypeService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
    {
        var result = await _jobTypeService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SearchRequest search, CancellationToken cancellationToken = default)
    {
        var result = await _jobTypeService.GetAllAsync(search, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var result = await _jobTypeService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
