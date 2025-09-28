using mogaERP.Domain.Contracts.HR.Staff;
using mogaERP.Domain.Interfaces.HR_Module;

namespace mogaERP.API.Controllers;
public class StaffController(IStaffService staffService) : BaseApiController
{
    private readonly IStaffService _staffService = staffService;

    [HttpPost]
    public async Task<IActionResult> CreateStaff([FromForm] StaffRequest request, CancellationToken cancellationToken)
    {
        var result = await _staffService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStaff(int id, [FromForm] StaffRequest request, CancellationToken cancellationToken)
    {
        var result = await _staffService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStaff(int id, CancellationToken cancellationToken)
    {
        var result = await _staffService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllStaff([FromQuery] SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        var result = await _staffService.GetAllAsync(searchRequest, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("counts")]
    public async Task<IActionResult> GetStaffCounts(CancellationToken cancellationToken)
    {
        var result = await _staffService.GetStaffCountsAsync(cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("inActive/{id}")]
    public async Task<IActionResult> InActiveStaff(int id, CancellationToken cancellationToken)
    {
        var result = await _staffService.InActiveStaffAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStaff(int id, CancellationToken cancellationToken)
    {
        var result = await _staffService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
