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
        var builder = new SqlBuilder();
        if (options.StartedOn.HasValue)
        {
            builder.Where("A.EndedOn >= @StartedOn", new { options.StartedOn });
        }

        if (options.EndedOn.HasValue)
        {
            builder.Where("A.StartedOn <= @EndedOn", new { options.EndedOn });
        }

        if (options.ApprovalStatus.HasValue)
        {
            builder.Where("A.ApprovalStatus = @ApprovalStatus", new { options.ApprovalStatus });
        }

        var sqlCount = builder.AddTemplate("SELECT COUNT(*) FROM HR.Leave AS A /**where**/");
        var itemCount = await connection.ExecuteScalarAsync<int>(sqlCount.RawSql, sqlCount.Parameters);
        var result = new PaginationResult<LeaveSummary>(options, itemCount);
        if (itemCount == 0)
        {
            return result;
        }

        var sql = builder.AddTemplate($@"
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
/**where**/
ORDER BY A.Id
OFFSET {result.Offset} ROWS
FETCH NEXT {result.Limit} ROWS ONLY
");
        var leaves = await connection.QueryAsync<LeaveSummary>(sql.RawSql, sql.Parameters);
        result.Items = leaves.ToList();

        return result;
    }

    public async Task<LeaveDetail> GetLeaveAsync(Guid leaveId)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = $@"
SELECT
    A.Id,
    A.Type,
    A.StartedOn,
    A.EndedOn,
    A.Reason,
    A.ApprovalStatus,
    A.CreatedOn,
    A.EmployeeId,
    B.Name AS EmployeeName
FROM HR.Leave AS A
INNER JOIN HR.Person AS B ON
    A.EmployeeId = B.Id AND
    B.Discriminator = N'Employee'
WHERE A.Id = @LeaveId
";
        var result = await connection.QuerySingleAsync<LeaveDetail>(sql, new { leaveId });

        return result;
    }
}
