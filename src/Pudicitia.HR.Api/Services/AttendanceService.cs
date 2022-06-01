using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Pudicitia.Hr;
using Pudicitia.HR.App.Attendance;

namespace Pudicitia.HR.Api.Services;

public class AttendanceService : Attendance.AttendanceBase
{
    private readonly AttendanceApp _attendanceApp;

    public AttendanceService(AttendanceApp attendanceApp)
    {
        _attendanceApp = attendanceApp ?? throw new ArgumentNullException(nameof(attendanceApp));
    }

    public override async Task<PaginateLeavesResponse> PaginateLeaves(
        PaginateLeavesRequest request,
        ServerCallContext context)
    {
        var options = new LeaveOptions
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
        };
        var leaves = await _attendanceApp.GetLeavesAsync(options);
        var items = leaves.Items.Select(x => new PaginateLeavesResponse.Types.Leave
        {
            Id = x.Id,
            Type = (int)x.Type,
            StartedOn = x.StartedOn.ToTimestamp(),
            EndedOn = x.EndedOn.ToTimestamp(),
            ApprovalStatus = (int)x.ApprovalStatus,
            EmployeeId = x.EmployeeId,
            EmployeeName = x.EmployeeName,
        });
        var result = new PaginateLeavesResponse
        {
            PageIndex = leaves.PageIndex,
            PageSize = leaves.PageSize,
            ItemCount = leaves.ItemCount,
        };
        result.Items.AddRange(items);

        return result;
    }
}
