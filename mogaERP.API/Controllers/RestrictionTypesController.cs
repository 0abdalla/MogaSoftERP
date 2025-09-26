using mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.API.Controllers;

public class RestrictionTypesController(IRestrictionTypeService restrictionTypeService) : BaseApiController
{
    private readonly IRestrictionTypeService _restrictionTypeService = restrictionTypeService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RestrictionTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _restrictionTypeService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        var result = await _restrictionTypeService.GetAllAsync(searchRequest, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _restrictionTypeService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RestrictionTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _restrictionTypeService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _restrictionTypeService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
