using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Pudicitia.Hr;
using Pudicitia.HR.App.Attendance;
using Pudicitia.HR.Domain;

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
            Page = request.Page,
            StartedOn = request.StartedOn?.ToDateTime(),
            EndedOn = request.EndedOn?.ToDateTime(),
            ApprovalStatus = (ApprovalStatus?)request.ApprovalStatus,
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
            Page = leaves.Page,
        };
        result.Items.AddRange(items);

        return result;
    }
}
