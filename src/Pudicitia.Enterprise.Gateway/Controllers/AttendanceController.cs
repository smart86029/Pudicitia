using Pudicitia.Enterprise.Gateway.Models.Attendance;

namespace Pudicitia.Enterprise.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "HumanResources")]
public class AttendanceController : ControllerBase
{
    private readonly Attendance.AttendanceClient _attendanceClient;

    public AttendanceController(Attendance.AttendanceClient attendanceClient)
    {
        _attendanceClient = attendanceClient ?? throw new ArgumentNullException(nameof(attendanceClient));
    }

    [HttpGet("Leaves")]
    public async Task<IActionResult> GetLeavesAsync([FromQuery] GetLeavesInput input)
    {
        var request = new PaginateLeavesRequest
        {
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
        };
        var response = await _attendanceClient.PaginateLeavesAsync(request);
        var result = new PaginationResult<LeaveSummary>
        {
            PageIndex = response.PageIndex,
            PageSize = response.PageSize,
            ItemCount = response.ItemCount,
            Items = response.Items
                .Select(x => new LeaveSummary
                {
                    Id = x.Id,
                    Type = x.Type,
                    StartedOn = x.StartedOn.ToDateTime(),
                    EndedOn = x.EndedOn.ToDateTime(),
                    ApprovalStatus = x.ApprovalStatus,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.EmployeeName,
                })
                .ToList(),
        };

        return Ok(result);
    }
}
