using mogaERP.Domain.Contracts.AccountingModule.AdditionNotice;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.API.Controllers;

public class AdditionNotificationsController(IAdditionNotificationService additionNotificationService) : BaseApiController
{
    private readonly IAdditionNotificationService _additionNotificationService = additionNotificationService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        var result = await _additionNotificationService.GetAllAsync(searchRequest, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _additionNotificationService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AdditionNotificationRequest request, CancellationToken cancellationToken)
    {
        var result = await _additionNotificationService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AdditionNotificationRequest request, CancellationToken cancellationToken)
    {
        var result = await _additionNotificationService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _additionNotificationService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}