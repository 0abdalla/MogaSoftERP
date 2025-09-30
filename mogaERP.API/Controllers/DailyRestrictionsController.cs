using mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.API.Controllers;

public class DailyRestrictionsController(IDailyRestrictionService service) : BaseApiController
{
    private readonly IDailyRestrictionService _dailyRestrictionService = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DailyRestrictionRequest request, CancellationToken cancellationToken)
    {
        var result = await _dailyRestrictionService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        var result = await _dailyRestrictionService.GetAllAsync(searchRequest, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _dailyRestrictionService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DailyRestrictionRequest request, CancellationToken cancellationToken)
    {
        var result = await _dailyRestrictionService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _dailyRestrictionService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("report/{accountId}")]
    public async Task<IActionResult> GetAccountReport(int accountId, [FromQuery] DateOnly fromDate, [FromQuery] DateOnly toDate, CancellationToken cancellationToken)
    {
        var result = await _dailyRestrictionService.GetAccountReportAsync(accountId, fromDate, toDate, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
