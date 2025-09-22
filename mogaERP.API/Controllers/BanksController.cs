using mogaERP.Domain.Contracts.AccountingModule.Bank;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.API.Controllers;

public class BanksController(IBankService bankService) : BaseApiController
{
    private readonly IBankService _bankService = bankService;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] BankRequest request, CancellationToken cancellationToken)
    {
        var result = await _bankService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        var result = await _bankService.GetAllAsync(searchRequest, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _bankService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BankRequest request, CancellationToken cancellationToken)
    {
        var result = await _bankService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _bankService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
