using mogaERP.Domain.Contracts.InventoryModule.Items;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.API.Controllers;

public class ItemGroupsController(IItemGroupService itemGroupService) : BaseApiController
{
    private readonly IItemGroupService _itemGroupService = itemGroupService;

    [HttpPost]
    public async Task<IActionResult> CreateItemGroup([FromBody] ItemGroupRequest request, CancellationToken cancellationToken)
    {
        var result = await _itemGroupService.CreateItemGroupAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateItemGroup(int id, [FromBody] ItemGroupRequest request, CancellationToken cancellationToken)
    {
        var result = await _itemGroupService.UpdateItemGroupAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteItemGroup(int id, CancellationToken cancellationToken)
    {
        var result = await _itemGroupService.DeleteItemGroupAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetItemGroupById(int id, CancellationToken cancellationToken)
    {
        var result = await _itemGroupService.GetItemGroupByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItemGroups([FromQuery] SearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _itemGroupService.GetAllItemGroupsAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
