using mogaERP.Domain.Contracts.AccountingModule.Treasury;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.API.Controllers;

public class TreasuriesController(ITreasuryService treasuryService) : BaseApiController
{
    private readonly ITreasuryService _treasuryService = treasuryService;

    [HttpPost("")]
    public async Task<IActionResult> CreateTreasury([FromBody] TreasuryRequest request, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.CreateTreasuryAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetTreasuries([FromQuery] SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.GetTreasuriesAsync(searchRequest, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTreasuryById(int id, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.GetTreasuryByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTreasury(int id, [FromBody] TreasuryRequest request, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.UpdateTreasuryAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTreasury(int id, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.DeleteTreasuryAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
