using mogaERP.Domain.Contracts.AccountingModule.FiscalYear;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.API.Controllers;
public class FiscalYearsController(IFiscalYearService fiscalYearService) : BaseApiController
{
    private readonly IFiscalYearService _fiscalYearService = fiscalYearService;

    [HttpPost("")]
    public async Task<IActionResult> CreateAsync([FromBody] FiscalYearRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _fiscalYearService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _fiscalYearService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllAsync([FromQuery] SearchRequest searchRequest, CancellationToken cancellationToken = default)
    {
        var result = await _fiscalYearService.GetAllAsync(searchRequest, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] FiscalYearRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _fiscalYearService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var result = await _fiscalYearService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
