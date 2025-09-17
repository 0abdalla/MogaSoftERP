using mogaERP.Domain.Contracts.InventoryModule.Items;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.API.Controllers;

public class MainGroupsController(IMainGroupService mainGroupService) : BaseApiController
{
    private readonly IMainGroupService _mainGroupService = mainGroupService;

    [HttpPost]
    public async Task<IActionResult> CreateMainGroup([FromBody] MainGroupRequest request, CancellationToken cancellationToken)
    {
        var result = await _mainGroupService.CreateMainGroupAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMainGroup(int id, [FromBody] MainGroupRequest request, CancellationToken cancellationToken)
    {
        var result = await _mainGroupService.UpdateMainGroupAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMainGroup(int id, CancellationToken cancellationToken)
    {
        var result = await _mainGroupService.DeleteMainGroupAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMainGroupById(int id, CancellationToken cancellationToken)
    {
        var result = await _mainGroupService.GetMainGroupByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMainGroups([FromQuery] SearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _mainGroupService.GetAllMainGroupsAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
