using mogaERP.Domain.Contracts.InventoryModule.DisbursementRequest;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.API.Controllers;

public class DisbursementRequestController(IDisbursementRequestService disbursementRequestService) : BaseApiController
{
    private readonly IDisbursementRequestService _disbursementRequestService = disbursementRequestService;

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] DisbursementRequestRequest request, CancellationToken cancellationToken)
    {
        var result = await _disbursementRequestService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] DisbursementRequestRequest request, CancellationToken cancellationToken)
    {
        var result = await _disbursementRequestService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _disbursementRequestService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _disbursementRequestService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] SearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _disbursementRequestService.GetAllAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}/approve")]
    public async Task<IActionResult> ApproveAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _disbursementRequestService.ApproveDisbursementRequestAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("approved")]
    public async Task<IActionResult> GetAllApprovedAsync([FromQuery] SearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _disbursementRequestService.GetAllApprovedAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
