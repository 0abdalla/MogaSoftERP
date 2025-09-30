using mogaERP.Domain.Contracts.SalesModule.Customer;
using mogaERP.Domain.Interfaces.SalesModule;

namespace mogaERP.API.Controllers;

public class CustomersController(ICustomerService customerService) : BaseApiController
{
    private readonly ICustomerService _customerService = customerService;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _customerService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        var result = await _customerService.GetAllAsync(searchRequest, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _customerService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _customerService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _customerService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
