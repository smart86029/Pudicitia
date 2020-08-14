using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pudicitia.Common.Domain;

namespace Pudicitia.HR.Domain.Employees
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<ICollection<Employee>> GetEmployeesAsync(int offset, int limit);

        Task<Employee> GetEmployeeAsync(Guid employeeId);

        Task<int> GetEmployeesCountAsync();

        void Add(Employee employee);

        void Update(Employee employee);
    }
}