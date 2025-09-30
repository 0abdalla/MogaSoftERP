using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using mogaERP.Domain.Contracts.SalesModule.SalesQuotation;
using mogaERP.Domain.Interfaces.SalesModule;
using mogaERP.Services.Services.SalesModule;

namespace mogaERP.API.Controllers;

public class SalesQuotationsController(ISalesQuotationService salesQuotationService) : BaseApiController
{
    private readonly ISalesQuotationService _salesQuotationService = salesQuotationService;

    [HttpPost]
    public async Task<IActionResult> CreateSalesQuotation([FromBody] SalesQuotationRequest request, CancellationToken cancellationToken)
    {
        var result = await _salesQuotationService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSalesQuotations([FromQuery] SearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _salesQuotationService.GetAllAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSalesQuotationById(int id, CancellationToken cancellationToken)
    {
        var result = await _salesQuotationService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSalesQuotation(int id, [FromBody] SalesQuotationRequest request, CancellationToken cancellationToken)
    {
        var result = await _salesQuotationService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSalesQuotation(int id, CancellationToken cancellationToken)
    {
        var result = await _salesQuotationService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
