using mogaERP.Domain.Contracts.AccountingModule.DebitNotice;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.API.Controllers;

public class DebitNotificationController(IDebitNoticeService debitNoticeService) : BaseApiController
{
    private readonly IDebitNoticeService _debitNoticeService = debitNoticeService;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] DebitNoticeRequest request, CancellationToken cancellationToken)
    {
        var result = await _debitNoticeService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        var result = await _debitNoticeService.GetAllAsync(searchRequest, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _debitNoticeService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DebitNoticeRequest request, CancellationToken cancellationToken)
    {
        var result = await _debitNoticeService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _debitNoticeService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
