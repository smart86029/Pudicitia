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

    public async Task<ICollection<DepartmentSummary>> GetDepartmentsAsync(DepartmentOptions options)
    {
        using var connection = new SqlConnection(_connectionString);
        var builder = new SqlBuilder();

        if (options.IsEnabled.HasValue)
        {
            builder.Where("A.IsEnabled = @IsEnabled", new { IsEnabled = options.IsEnabled.Value });
        }

        var sql = builder.AddTemplate($@"
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
/**where**/
");
        var departments = await connection.QueryAsync<DepartmentSummary>(sql.RawSql, sql.Parameters);

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

        var childCount = await _departmentRepository.GetCountAsync(departmentId);
        if (childCount > 0)
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
        builder.Where("A.Discriminator = N'Employee'");

        if (!string.IsNullOrWhiteSpace(options.Name))
        {
            builder.Where("A.Name LIKE @Name", new { Name = $"{options.Name}%" });
        }

        if (options.DepartmentId.HasValue)
        {
            builder.Where("A.DepartmentId = @DepartmentId", new { options.DepartmentId });
        }

        var sqlCount = builder.AddTemplate("SELECT COUNT(*) FROM HR.Person AS A /**where**/");
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
    A.UserId,
    B.Name AS DepartmentName,
    C.Title AS JobTitle
FROM HR.Person AS A
LEFT JOIN HR.Department AS B ON B.Id = A.DepartmentId
LEFT JOIN HR.Job AS C ON C.Id = A.JobId
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
        using var connection = new SqlConnection(_connectionString);
        var sql = $@"
SELECT
    A.Id,
    A.Name,
    A.DisplayName,
    A.BirthDate,
    A.Gender,
    A.MaritalStatus,
    A.UserId,
    B.Name AS DepartmentName,
    C.Title AS JobTitle
FROM HR.Person AS A
LEFT JOIN HR.Department AS B ON B.Id = A.DepartmentId
LEFT JOIN HR.Job AS C ON C.Id = A.JobId
WHERE A.Id = @EmployeeId
ORDER BY A.Id
";
        var result = await connection.QuerySingleAsync<EmployeeDetail>(sql, new { employeeId });

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

    public async Task<PaginationResult<JobSummary>> GetJobsAsync(JobOptions options)
    {
        using var connection = new SqlConnection(_connectionString);
        var builder = new SqlBuilder();

        if (!string.IsNullOrWhiteSpace(options.Title))
        {
            builder.Where("A.Title LIKE @Title", new { Title = $"{options.Title}%" });
        }

        if (options.IsEnabled.HasValue)
        {
            builder.Where("A.IsEnabled = @IsEnabled", new { IsEnabled = options.IsEnabled.Value });
        }

        var sqlCount = builder.AddTemplate("SELECT COUNT(*) FROM HR.Job AS A /**where**/");
        var itemCount = await connection.ExecuteScalarAsync<int>(sqlCount.RawSql, sqlCount.Parameters);
        var result = new PaginationResult<JobSummary>(options, itemCount);
        var sql = builder.AddTemplate($@"
SELECT
    A.Id,
    A.Title,
    A.IsEnabled,
    B.EmployeeCount
FROM HR.Job AS A
LEFT JOIN (
	SELECT JobId, COUNT(*) AS EmployeeCount
	FROM HR.JobChange
	WHERE EndOn IS NULL OR EndOn >= GETDATE()
	GROUP BY JobId
) AS B ON A.Id = B.JobId
/**where**/
ORDER BY A.Id
OFFSET {result.Offset} ROWS
FETCH NEXT {result.Limit} ROWS ONLY
");
        var jobs = await connection.QueryAsync<JobSummary>(sql.RawSql, sql.Parameters);
        result.Items = jobs.ToList();

        return result;
    }

    public async Task<JobDetail> GetJobAsync(Guid jobId)
    {
        var job = await _jobRepository.GetJobAsync(jobId);
        var result = new JobDetail
        {
            Id = job.Id,
            Title = job.Title,
            IsEnabled = job.IsEnabled,
        };

        return result;
    }

    public async Task<Guid> CreateJobAsync(CreateJobCommand command)
    {
        var job = new Job(command.Title, command.IsEnabled);

        _jobRepository.Add(job);
        await _unitOfWork.CommitAsync();

        return job.Id;
    }

    public async Task UpdateJobAsync(UpdateJobCommand command)
    {
        var job = await _jobRepository.GetJobAsync(command.Id);

        job.UpdateTitle(command.Title);
        if (command.IsEnabled)
        {
            job.Enable();
        }
        else
        {
            job.Disable();
        }

        _jobRepository.Update(job);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteJobAsync(Guid jobId)
    {
        var employeeCount = await _employeeRepository.GetCountByJobAsync(jobId);
        if (employeeCount > 0)
        {
            throw new InvalidCommandException("Assigned job can not be deleted");
        }

        var job = await _jobRepository.GetJobAsync(jobId);

        _jobRepository.Remove(job);
        await _unitOfWork.CommitAsync();
    }
}
