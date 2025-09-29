using mogaERP.Domain.Contracts.AccountingModule.CostCenterTree;
using mogaERP.Domain.Entities;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.API.Controllers;

public class CostCenterTreeController : BaseApiController
{
    private readonly ICostCenterTreeService _costCenterTreeService;

    public CostCenterTreeController(ICostCenterTreeService costCenterTreeService)
    {
        _costCenterTreeService = costCenterTreeService;
    }

    [HttpPost]
    [Route("CreateNewCostCenter")]

    public async Task<IActionResult> CreateNewCostCenter(CostCenterTreeModel Model)
    {
        var results = await _costCenterTreeService.CreateNewCostCenter(Model);
        return results.IsSuccess ? Ok(results) : BadRequest(results);
    }

    [HttpPost]
    [Route("UpdateCostCenterTree")]

    public async Task<IActionResult> UpdateCostCenterTree(int CostCenterId, CostCenterTreeModel Model)
    {
        var results = await _costCenterTreeService.UpdateCostCenterTree(CostCenterId, Model);
        return results.IsSuccess ? Ok(results) : BadRequest(results);
    }
    [HttpGet]
    [Route("GenerateCostCenterNumber")]
    public IActionResult GenerateCostCenterNumber(int? ParentCostCenterId)
    {
        int? id = ParentCostCenterId == 0 ? null : ParentCostCenterId;
        var results = _costCenterTreeService.GenerateCostCenterNumber(id);
        return Ok(results);
    }
    [HttpGet]
    [Route("DeleteCostCenterTree")]

    public async Task<IActionResult> DeleteCostCenterTree(int CostCenterId)
    {
        var results = await _costCenterTreeService.DeleteCostCenterTree(CostCenterId);
        return results.IsSuccess ? Ok(results) : BadRequest(results);
    }


    [HttpGet]
    [Route("GetCostCenterTreeHierarchicalData")]
    public IActionResult GetCostCenterTreeHierarchicalData(string? SearchText)
    {
        var results = _costCenterTreeService.GetCostCenterTreeHierarchicalData(SearchText);
        return Ok(results);
    }



    [HttpGet]
    [Route("GetCostCenterTreeData")]
    public async Task<List<CostCenterTree>> GetCostCenterTreeData(string? SearchText)
    {
        return await _costCenterTreeService.GetCostCenterTreeDataBySearch(SearchText);
    }
}
