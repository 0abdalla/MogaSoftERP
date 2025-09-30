using mogaERP.Domain.Contracts.HR.EmployeeAdvance;
using mogaERP.Domain.Interfaces.HR_Module;

namespace mogaERP.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmployeeAdvancesController : ControllerBase
{
    private readonly IEmployeeAdvancesService _employeeAdvancesService;
    public EmployeeAdvancesController(IEmployeeAdvancesService employeeAdvancesService)
    {
        _employeeAdvancesService = employeeAdvancesService;
    }

    [HttpPost]
    [Route("GetAdvancesByEmployeeId")]
    public IActionResult GetAdvancesByEmployeeId(int EmployeeId, SearchRequest SearchModel)
    {
        var data = _employeeAdvancesService.GetAdvancesByEmployeeId(SearchModel, EmployeeId).ToList();
        var result = new PagedResponse<EmployeeAdvanceModel>(data, data.FirstOrDefault()?.TotalCount ?? 0, SearchModel.PageNumber, SearchModel.PageSize);
        //{
        //    Data = data,
        //    TotalCount = data.FirstOrDefault()?.TotalCount ?? 0,
        //    PageNumber = SearchModel?.PageNumber ?? 1,
        //    PageSize = SearchModel?.PageSize ?? 1,
        //};
        return Ok(result);
    }

    [HttpPost]
    [Route("AddNewEmployeeAdvance")]
    public async Task<IActionResult> AddNewEmployeeAdvance(int EmployeeId, EmployeeAdvanceModel model)
    {
        var result = await _employeeAdvancesService.AddNewEmployeeAdvance(EmployeeId, model);
        return Ok(result);
    }

    [HttpPost]
    [Route("EditEmployeeAdvance")]
    public async Task<IActionResult> EditEmployeeAdvance(int EmployeeId, EmployeeAdvanceModel model)
    {
        var result = await _employeeAdvancesService.EditEmployeeAdvance(EmployeeId, model);

        return Ok(result);
    }

    [HttpGet]
    [Route("ApproveEmployeeAdvance")]
    public async Task<IActionResult> ApproveEmployeeAdvance(int EmployeeAdvanceId, bool IsApproved)
    {
        var result = await _employeeAdvancesService.ApproveEmployeeAdvance(EmployeeAdvanceId, IsApproved);
        return Ok(result);
    }

    [HttpGet]
    [Route("DeleteEmployeeAdvance")]
    public async Task<IActionResult> DeleteEmployeeAdvance(int EmployeeAdvanceId)
    {
        var result = await _employeeAdvancesService.DeleteEmployeeAdvance(EmployeeAdvanceId);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetAdvanceTypesSelector")]
    public IActionResult GetAdvanceTypesSelector()
    {
        var result = _employeeAdvancesService.GetAdvanceTypesSelector();
        return Ok(result);
    }
}