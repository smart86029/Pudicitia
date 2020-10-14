using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pudicitia.Common.App;
using Pudicitia.HR.Domain;
using Pudicitia.HR.Domain.Departments;
using Pudicitia.HR.Domain.Employees;
using Pudicitia.HR.Domain.Jobs;

namespace Pudicitia.HR.App.Organization
{
    public class OrganizationApp
    {
        private readonly IHRUnitOfWork unitOfWork;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IJobRepository jobRepository;

        public OrganizationApp(
            IHRUnitOfWork unitOfWork,
            IDepartmentRepository departmentRepository,
            IEmployeeRepository employeeRepository,
            IJobRepository jobRepository)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            this.employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            this.jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
        }

        public async Task<ICollection<DepartmentSummary>> GetDepartmentsAsync()
        {
            var departments = await departmentRepository.GetDepartmentsAsync();
            var result = departments
                .Select(x => new DepartmentSummary
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId,
                })
                .ToList();

            return result;
        }

        public async Task<Guid> CreateDepartmentAsync(CreateDepartmentCommand command)
        {
            var department = new Department(command.Name, command.IsEnabled, command.ParentId);

            departmentRepository.Add(department);
            await unitOfWork.CommitAsync();

            return department.Id;
        }

        public async Task UpdateDepartmentAsync(UpdateDepartmentCommand command)
        {
            var department = await departmentRepository.GetDepartmentAsync(command.Id);

            department.UpdateName(command.Name);
            departmentRepository.Update(department);
            await unitOfWork.CommitAsync();
        }

        public async Task DeleteDepartmentAsync(Guid departmentId)
        {
            var department = await departmentRepository.GetDepartmentAsync(departmentId);
            if (department.ParentId == default)
                throw new InvalidCommandException("Root department can not be deleted");

            var departments = await departmentRepository.GetDepartmentsAsync();
            if (departments.Any(x => x.ParentId == departmentId))
                throw new InvalidCommandException("Has child departments can not be deleted");

            departmentRepository.Remove(department);
            await unitOfWork.CommitAsync();
        }

        public async Task<PaginationResult<EmployeeSummary>> GetEmployeesAsync(EmployeeOptions options)
        {
            var count = await employeeRepository.GetEmployeesCountAsync(options.DepartmentId);
            var employees = await employeeRepository.GetEmployeesAsync(options.DepartmentId, options.Offset, options.Limit);
            var result = new PaginationResult<EmployeeSummary>
            {
                ItemCount = count,
                Items = employees
                    .Select(x => new EmployeeSummary
                    {
                        Id = x.Id,
                        Name = x.Name,
                        DisplayName = x.DisplayName,
                        DepartmentId = x.DepartmentId,
                        JobId = x.JobId,
                    })
                    .ToList(),
            };

            return result;
        }

        public async Task<EmployeeDetail> GetEmployeeAsync(Guid employeeId)
        {
            var employee = await employeeRepository.GetEmployeeAsync(employeeId);
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

        public async Task<ICollection<JobSummary>> GetJobsAsync()
        {
            var jobs = await jobRepository.GetJobsAsync();
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
}