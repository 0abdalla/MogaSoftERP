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


    [HttpPost("assign-treasury-to-staff/{treasuryId}/{staffId}")]
    //[Authorize(Roles = "SystemAdmin")]
    public async Task<IActionResult> AssignTreasury(int treasuryId, int staffId, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.AssignTreasuryToStaffAsync(staffId, treasuryId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("transactions/{treasuryId}")]
    public async Task<IActionResult> GetTreasuryTransactions(int treasuryId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.GetTreasuryTransactionsAsync(treasuryId, fromDate, toDate, cancellationToken);
        return Ok(result);
    }

    [HttpGet("movement/{id}")]
    public async Task<IActionResult> GetTreasuryMovementById(int id, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.GetTreasuryMovementByIdAsyncV1(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("movement/{id}/re-enable")]
    public async Task<IActionResult> EnableTreasuryMovement(int id, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.EnableTreasuryMovementAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("movement/{id}/re-disable")]
    public async Task<IActionResult> TreasuryMovement(int id, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.ReDisableTreasuryMovementAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("movement/{treasuryId}/disable")]
    public async Task<IActionResult> DisableTreasuryMovement(int treasuryId, [FromQuery] DateOnly closeInDate, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.DisableTreasuryMovementAsync(treasuryId, closeInDate, cancellationToken);
        return Ok(result);
    }

    [HttpGet("all-movements")]
    public async Task<IActionResult> GetAllMovements(CancellationToken cancellationToken)
    {
        var result = await _treasuryService.GetAllMovementsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("movement/enabled")]
    public async Task<IActionResult> GetEnabledTreasuries(CancellationToken cancellationToken)
    {
        var result = await _treasuryService.GetEnabledTreasuriesMovementsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("movement/disabled")]
    public async Task<IActionResult> GetDisabledTreasuries(CancellationToken cancellationToken)
    {
        var result = await _treasuryService.GetDisabledTreasuriesMovementsAsync(cancellationToken);
        return Ok(result);
    }
}
