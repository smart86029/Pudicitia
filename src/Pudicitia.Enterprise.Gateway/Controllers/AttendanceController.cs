using Pudicitia.Enterprise.Gateway.Models.Attendance;
using Pudicitia.HR;

namespace Pudicitia.Enterprise.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "HumanResources")]
public class AttendanceController : ControllerBase
{
    private readonly Attendance.AttendanceClient _attendanceClient;

    public AttendanceController(Attendance.AttendanceClient attendanceClient)
    {
        _attendanceClient = attendanceClient;
    }

    [HttpGet("Leaves")]
    public async Task<IActionResult> GetLeavesAsync([FromQuery] GetLeavesInput input)
    {
        var request = new PaginateLeavesRequest
        {
            Page = input,
            StartedOn = input.StartedOn?.ToTimestamp(),
            EndedOn = input.EndedOn?.ToTimestamp(),
            ApprovalStatus = input?.ApprovalStatus,
        };
        var response = await _attendanceClient.PaginateLeavesAsync(request);
        var result = new PaginationResult<LeaveSummary>
        {
            Page = response.Page,
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

    [HttpGet("Leaves/{id}")]
    public async Task<IActionResult> GetLeavesAsync([FromRoute] Guid id)
    {
        var request = new GetLeaveRequest
        {
            Id = id,
        };
        var response = await _attendanceClient.GetLeaveAsync(request);
        var result = new LeaveDetail
        {
            Id = response.Id,
            Type = response.Type,
            StartedOn = response.StartedOn.ToDateTime(),
            EndedOn = response.EndedOn.ToDateTime(),
            Reason = response.Reason,
            ApprovalStatus = response.ApprovalStatus,
            CreatedOn = response.CreatedOn.ToDateTime(),
            EmployeeId = response.EmployeeId,
            EmployeeName = response.EmployeeName,
        };

        return Ok(result);
    }
}
