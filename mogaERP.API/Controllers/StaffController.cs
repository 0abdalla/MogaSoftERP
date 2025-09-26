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
}
