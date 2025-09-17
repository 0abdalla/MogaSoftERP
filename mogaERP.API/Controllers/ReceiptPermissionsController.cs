using mogaERP.Domain.Contracts.InventoryModule.ReceiptPermissions;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.API.Controllers;

public class ReceiptPermissionsController(IReceiptPermissionService receiptPermissionService) : BaseApiController
{
    private readonly IReceiptPermissionService _receiptPermissionService = receiptPermissionService;

    [HttpPost]
    public async Task<IActionResult> CreateReceiptPermission([FromBody] ReceiptPermissionRequest request, CancellationToken cancellationToken)
    {
        var result = await _receiptPermissionService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReceiptPermission(int id, CancellationToken cancellationToken)
    {
        var result = await _receiptPermissionService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReceiptPermissions([FromQuery] SearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _receiptPermissionService.GetAllAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReceiptPermissionById(int id, CancellationToken cancellationToken)
    {
        var result = await _receiptPermissionService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReceiptPermission(int id, [FromBody] ReceiptPermissionRequest request, CancellationToken cancellationToken)
    {
        var result = await _receiptPermissionService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
