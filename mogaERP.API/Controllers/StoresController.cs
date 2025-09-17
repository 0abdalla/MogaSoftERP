using mogaERP.Domain.Contracts.InventoryModule.Stores;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.API.Controllers;

public class StoresController(IStoreService storeService) : BaseApiController
{
    private readonly IStoreService _storeService = storeService;

    [HttpPost]
    public async Task<IActionResult> CreateStore([FromBody] StoreRequest request, CancellationToken cancellationToken)
    {
        var result = await _storeService.CreateStoreAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateStore(int id, [FromBody] StoreRequest request, CancellationToken cancellationToken)
    {
        var result = await _storeService.UpdateStoreAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStore(int id, CancellationToken cancellationToken)
    {
        var result = await _storeService.DeleteStoreAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetStoreById(int id, CancellationToken cancellationToken)
    {
        var result = await _storeService.GetStoreByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStores([FromQuery] SearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _storeService.GetAllStoresAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
