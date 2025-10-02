using mogaERP.Domain.Contracts.SalesModule.SalesInvoices;
using mogaERP.Domain.Interfaces.SalesModule;

namespace mogaERP.API.Controllers;

public class SalesInvoicesController(ISalesInvoiceService salesInvoiceService) : BaseApiController
{
    private readonly ISalesInvoiceService _salesInvoiceService = salesInvoiceService;

    [HttpPost]
    public async Task<IActionResult> CreateSalesInvoice([FromBody] SalesInvoiceRequest request, CancellationToken cancellationToken)
    {
        var result = await _salesInvoiceService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSalesInvoices([FromQuery] SearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _salesInvoiceService.GetAllAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSalesInvoiceById(int id, CancellationToken cancellationToken)
    {
        var result = await _salesInvoiceService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSalesInvoice(int id, [FromBody] SalesInvoiceRequest request, CancellationToken cancellationToken)
    {
        var result = await _salesInvoiceService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSalesInvoice(int id, CancellationToken cancellationToken)
    {
        var result = await _salesInvoiceService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
