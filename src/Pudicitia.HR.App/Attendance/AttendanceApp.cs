using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using Pudicitia.Common.Dapper;
using Pudicitia.HR.Domain.Leaves;

namespace Pudicitia.HR.App.Attendance;

public class AttendanceApp
{
    private readonly string _connectionString;
    private readonly IHRUnitOfWork _unitOfWork;
    private readonly ILeaveRepository _leaveRepository;

    public AttendanceApp(
        IOptions<DapperOptions> options,
        IHRUnitOfWork unitOfWork,
        ILeaveRepository leaveRepository)
    {
        _connectionString = options.Value.ConnectionString;
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _leaveRepository = leaveRepository ?? throw new ArgumentNullException(nameof(leaveRepository));
    }

    public async Task<PaginationResult<LeaveSummary>> GetLeavesAsync(LeaveOptions options)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlCount = "SELECT COUNT(*) FROM HR.Leave";
        var itemCount = await connection.ExecuteScalarAsync<int>(sqlCount);
        var result = new PaginationResult<LeaveSummary>(options, itemCount);
        if (itemCount == 0)
        {
            return result;
        }

        var sql = $@"
SELECT
    A.Id,
    A.Type,
    A.StartedOn,
    A.EndedOn,
    A.ApprovalStatus,
    A.EmployeeId,
    B.Name AS EmployeeName
FROM HR.Leave AS A
INNER JOIN HR.Person AS B ON
    A.EmployeeId = B.Id AND
    B.Discriminator = N'Employee'
ORDER BY A.Id
OFFSET {result.Offset} ROWS
FETCH NEXT {result.Limit} ROWS ONLY
";
        var leaves = await connection.QueryAsync<LeaveSummary>(sql);
        result.Items = leaves.ToList();

        return result;
    }
}
