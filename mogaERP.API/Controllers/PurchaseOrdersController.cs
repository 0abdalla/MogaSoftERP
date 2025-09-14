using mogaERP.Domain.Contracts.ProcurementModule.PurchaseOrder;
using mogaERP.Domain.Interfaces.ProcurementModule;

namespace mogaERP.API.Controllers;

public class PurchaseOrdersController(IPurchaseOrderService purchaseOrderService) : BaseApiController
{
    private readonly IPurchaseOrderService _purchaseOrderService = purchaseOrderService;

    [HttpPost]
    public async Task<IActionResult> CreatePurchaseOrder([FromBody] PurchaseOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await _purchaseOrderService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPurchaseOrders([FromQuery] SearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _purchaseOrderService.GetAllAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPurchaseOrderById(int id, CancellationToken cancellationToken)
    {
        var result = await _purchaseOrderService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePurchaseOrder(int id, [FromBody] PurchaseOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await _purchaseOrderService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePurchaseOrder(int id, CancellationToken cancellationToken)
    {
        var result = await _purchaseOrderService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
