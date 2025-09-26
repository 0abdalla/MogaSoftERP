using mogaERP.Domain.Contracts.HR.JobTitle;
using mogaERP.Domain.Interfaces.HR_Module;

namespace mogaERP.API.Controllers;

public class JobTitlesController(IJobTitleService jobTitleService) : BaseApiController
{
    private readonly IJobTitleService _jobTitleService = jobTitleService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] JobTitleRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _jobTitleService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] JobTitleRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _jobTitleService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
    {
        var result = await _jobTitleService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SearchRequest search, CancellationToken cancellationToken = default)
    {
        var result = await _jobTitleService.GetAllAsync(search, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var result = await _jobTitleService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
