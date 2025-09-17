using mogaERP.Domain.Contracts.InventoryModule.Stores;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.API.Controllers;

public class StoreTypesController(IStoreTypeService storeTypeService) : BaseApiController
{
    private readonly IStoreTypeService _storeTypeService = storeTypeService;

    [HttpGet]
    public async Task<IActionResult> GetAllStoreTypes([FromQuery] SearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _storeTypeService.GetAllAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStoreTypeById(int id, CancellationToken cancellationToken)
    {
        var result = await _storeTypeService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStoreType([FromBody] StoreTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _storeTypeService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStoreType(int id, [FromBody] StoreTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _storeTypeService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStoreType(int id, CancellationToken cancellationToken)
    {
        var result = await _storeTypeService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
