using Pudicitia.Enterprise.Gateway.Models.Schedule;
using Pudicitia.HR;

namespace Pudicitia.Enterprise.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ScheduleController : ControllerBase
{
    private readonly Attendance.AttendanceClient _attendanceClient;

    public ScheduleController(Attendance.AttendanceClient attendanceClient)
    {
        _attendanceClient = attendanceClient;
    }

    [HttpGet("Events")]
    public async Task<IActionResult> GetEvents([FromQuery] GetEventsInput input)
    {
        var request = new ListLeavesRequest
        {
            UserId = HttpContext.GetUserId(),
            StartedOn = input.StartedOn.ToTimestamp(),
            EndedOn = input.EndedOn.ToTimestamp(),
        };
        var response = await _attendanceClient.ListLeavesAsync(request);
        var result = response.Items
            .Select(x => new EventSummary
            {
                Id = x.Id,
                Title = x.Title,
                StartedOn = x.StartedOn.ToDateTime(),
                EndedOn = x.EndedOn.ToDateTime(),
                IsAllDay = x.IsAllDay,
            })
            .ToList();

        return Ok(result);
    }
}
