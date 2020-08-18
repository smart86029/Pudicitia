using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pudicitia.Common.App;
using Pudicitia.HR.Domain;
using Pudicitia.HR.Domain.Departments;

namespace Pudicitia.HR.App.Organization
{
    public class OrganizationApp
    {
        private readonly IHRUnitOfWork unitOfWork;
        private readonly IDepartmentRepository departmentRepository;

        public OrganizationApp(
            IHRUnitOfWork unitOfWork,
            IDepartmentRepository departmentRepository)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        }

        public async Task<ICollection<DepartmentSummary>> GetDepartmentsAsync()
        {
            var departments = await departmentRepository.GetDepartmentsAsync();
            var result = departments
                .Select(d => new DepartmentSummary
                {
                    Id = d.Id,
                    Name = d.Name,
                    ParentId = d.ParentId,
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
            if (department == default)
                return;

            if (department.ParentId == default)
                throw new InvalidCommandException("Root department can not be deleted");

            var departments = await departmentRepository.GetDepartmentsAsync();
            if (departments.Any(d => d.ParentId == departmentId))
                throw new InvalidCommandException("Has child departments can not be deleted");

            departmentRepository.Remove(department);
            await unitOfWork.CommitAsync();
        }
    }
}
