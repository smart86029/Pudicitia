using Pudicitia.HR.Domain.Departments;
using Pudicitia.HR.Domain.Employees;
using Pudicitia.HR.Domain.Jobs;

namespace Pudicitia.HR.App.Organization;

public class OrganizationApp
{
    private readonly IHRUnitOfWork _unitOfWork;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IJobRepository _jobRepository;

    public OrganizationApp(
        IHRUnitOfWork unitOfWork,
        IDepartmentRepository departmentRepository,
        IEmployeeRepository employeeRepository,
        IJobRepository jobRepository)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
    }

    public async Task<ICollection<DepartmentSummary>> GetDepartmentsAsync()
    {
        var departments = await _departmentRepository.GetDepartmentsAsync();
        var result = departments
            .Select(x => new DepartmentSummary
            {
                Id = x.Id,
                Name = x.Name,
                IsEnabled = x.IsEnabled,
                ParentId = x.ParentId,
            })
            .ToList();

        return result;
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
        var itemCount = await _employeeRepository.GetEmployeesCountAsync(options.DepartmentId);
        var result = new PaginationResult<EmployeeSummary>(options, itemCount);
        if (itemCount == 0)
        {
            return result;
        }

        var count = await _employeeRepository.GetEmployeesCountAsync(options.DepartmentId);
        var employees = await _employeeRepository.GetEmployeesAsync(options.DepartmentId, result.Offset, result.Limit);
        result.Items = employees
            .Select(x => new EmployeeSummary
            {
                Id = x.Id,
                Name = x.Name,
                DisplayName = x.DisplayName,
                DepartmentId = x.DepartmentId,
                JobId = x.JobId,
            })
            .ToList();

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
        var result = jobs
            .Select(x => new JobSummary
            {
                Id = x.Id,
                Title = x.Title,
                IsEnabled = x.IsEnabled,
            })
            .ToList();

        return result;
    }
}
