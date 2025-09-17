using mogaERP.Domain.Contracts.InventoryModule.Items;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.API.Controllers;

public class ItemUnitsController(IItemUnitService itemUnitService) : BaseApiController
{
    private readonly IItemUnitService _itemUnitService = itemUnitService;

    [HttpGet]
    public async Task<IActionResult> GetAllItemUnits([FromQuery] SearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _itemUnitService.GetAllItemUnitsAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetItemUnitById(int id, CancellationToken cancellationToken)
    {
        var result = await _itemUnitService.GetItemUnitByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateItemUnit(int id, [FromBody] ItemUnitRequest request, CancellationToken cancellationToken)
    {
        var result = await _itemUnitService.UpdateItemUnitAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteItemUnit(int id, CancellationToken cancellationToken)
    {
        var result = await _itemUnitService.DeleteItemUnitAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateItemUnit([FromBody] ItemUnitRequest request, CancellationToken cancellationToken)
    {
        var result = await _itemUnitService.CreateItemUnitAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
