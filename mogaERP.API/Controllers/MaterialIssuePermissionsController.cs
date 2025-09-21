using mogaERP.Domain.Contracts.InventoryModule.MaterialIssues;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.API.Controllers;

public class MaterialIssuePermissionsController(IMaterialIssuePermissionService service) : BaseApiController
{
    private readonly IMaterialIssuePermissionService _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MaterialIssuePermissionRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MaterialIssuePermissionRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        return Ok(result);
    }
}
