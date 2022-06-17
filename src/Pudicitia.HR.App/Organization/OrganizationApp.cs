using Pudicitia.HR.Domain.Departments;
using Pudicitia.HR.Domain.Employees;
using Pudicitia.HR.Domain.Jobs;

namespace Pudicitia.HR.App.Organization;

public class OrganizationApp
{
    private readonly string _connectionString;
    private readonly IHRUnitOfWork _unitOfWork;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IJobRepository _jobRepository;

    public OrganizationApp(
        IOptions<DapperOptions> options,
        IHRUnitOfWork unitOfWork,
        IDepartmentRepository departmentRepository,
        IEmployeeRepository employeeRepository,
        IJobRepository jobRepository)
    {
        _connectionString = options.Value.ConnectionString;
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
    }

    public async Task<ICollection<DepartmentSummary>> GetDepartmentsAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = $@"
SELECT
    A.Id,
    A.Name,
    A.ParentId,
    A.IsEnabled,
	D.Name AS HeadName,
    B.EmployeeCount
FROM HR.Department AS A
LEFT JOIN (
	SELECT DepartmentId, COUNT(*) AS EmployeeCount
	FROM HR.JobChange
	WHERE EndOn IS NULL OR EndOn >= GETDATE()
	GROUP BY DepartmentId
) AS B ON A.Id = B.DepartmentId
LEFT JOIN (
	SELECT DepartmentId, EmployeeId
	FROM HR.JobChange
	WHERE (EndOn IS NULL OR EndOn >= GETDATE()) AND IsHead = 1
) AS C ON A.Id = C.DepartmentId
LEFT JOIN (
	SELECT Id, Name
	FROM HR.Person
) AS D ON C.EmployeeId = D.Id
";
        var departments = await connection.QueryAsync<DepartmentSummary>(sql);

        return departments.ToList();
    }

    public async Task<DepartmentDetail> GetDepartmentAsync(Guid departmentId)
    {
        var department = await _departmentRepository.GetDepartmentAsync(departmentId);
        var result = new DepartmentDetail
        {
            Id = department.Id,
            Name = department.Name,
            IsEnabled = department.IsEnabled,
            ParentId = department.ParentId,
        };

        return result;
    }

    public async Task<Guid> CreateDepartmentAsync(CreateDepartmentCommand command)
    {
        var department = new Department(command.Name, command.IsEnabled, command.ParentId);

        _departmentRepository.Add(department);
        await _unitOfWork.CommitAsync();

        return department.Id;
    }

    public async Task UpdateDepartmentAsync(UpdateDepartmentCommand command)
    {
        var department = await _departmentRepository.GetDepartmentAsync(command.Id);

        department.UpdateName(command.Name);
        if (command.IsEnabled)
        {
            department.Enable();
        }
        else
        {
            department.Disable();
        }

        _departmentRepository.Update(department);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteDepartmentAsync(Guid departmentId)
    {
        var department = await _departmentRepository.GetDepartmentAsync(departmentId);
        if (department.ParentId is null)
        {
            throw new InvalidCommandException("Root department can not be deleted");
        }

        var departments = await _departmentRepository.GetDepartmentsAsync();
        if (departments.Any(x => x.ParentId == departmentId))
        {
            throw new InvalidCommandException("Has child departments can not be deleted");
        }

        _departmentRepository.Remove(department);
        await _unitOfWork.CommitAsync();
    }

    public async Task<PaginationResult<EmployeeSummary>> GetEmployeesAsync(EmployeeOptions options)
    {
        using var connection = new SqlConnection(_connectionString);
        var builder = new SqlBuilder();
        builder.LeftJoin("HR.JobChange AS B ON A.Id = B.EmployeeId");
        builder.Where("A.Discriminator = N'Employee'");

        if (options.DepartmentId != Guid.Empty)
        {
            builder.Where("B.DepartmentId = @DepartmentId", new { options.DepartmentId });
        }

        var sqlCount = builder.AddTemplate("SELECT COUNT(*) FROM HR.Person AS A /**leftjoin**//**where**/");
        var itemCount = await connection.ExecuteScalarAsync<int>(sqlCount.RawSql, sqlCount.Parameters);
        var result = new PaginationResult<EmployeeSummary>(options, itemCount);
        if (itemCount == 0)
        {
            return result;
        }

        var sql = builder.AddTemplate($@"
SELECT
    A.Id,
    A.Name,
    A.DisplayName,
    C.Name AS DepartmentName,
    D.Title AS JobTitle
FROM HR.Person AS A
/**leftjoin**/
LEFT JOIN HR.Department AS C ON C.Id = B.DepartmentId
LEFT JOIN HR.Job AS D ON D.Id = B.JobId
/**where**/
ORDER BY A.Id
OFFSET {result.Offset} ROWS
FETCH NEXT {result.Limit} ROWS ONLY
");
        var employees = await connection.QueryAsync<EmployeeSummary>(sql.RawSql, sql.Parameters);
        result.Items = employees.ToList();

        return result;
    }

    public async Task<EmployeeDetail> GetEmployeeAsync(Guid employeeId)
    {
        var employee = await _employeeRepository.GetEmployeeAsync(employeeId);
        var result = new EmployeeDetail
        {
            Id = employee.Id,
            Name = employee.Name,
            DisplayName = employee.DisplayName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            MaritalStatus = employee.MaritalStatus,
        };

        return result;
    }

    public async Task<Guid> CreateEmployeeAsync(CreateEmployeeCommand command)
    {
        var employee = new Employee(
            command.Name,
            command.DisplayName,
            command.BirthDate,
            command.Gender,
            command.MaritalStatus);

        _employeeRepository.Add(employee);
        await _unitOfWork.CommitAsync();

        return employee.Id;
    }

    public async Task UpdateEmployeeAsync(UpdateEmployeeCommand command)
    {
        var employee = await _employeeRepository.GetEmployeeAsync(command.Id);

        employee.UpdateName(command.Name);
        employee.UpdateDisplayName(command.DisplayName);
        employee.UpdateBirthDate(command.BirthDate);
        employee.UpdateGender(command.Gender);
        employee.UpdateMaritalStatus(command.MaritalStatus);

        _employeeRepository.Update(employee);
        await _unitOfWork.CommitAsync();
    }

    public async Task<ICollection<JobSummary>> GetJobsAsync()
    {
        var jobs = await _jobRepository.GetJobsAsync();
        var results = jobs
            .Select(x => new JobSummary
            {
                Id = x.Id,
                Title = x.Title,
                IsEnabled = x.IsEnabled,
            })
            .ToList();

        return results;
    }
}
