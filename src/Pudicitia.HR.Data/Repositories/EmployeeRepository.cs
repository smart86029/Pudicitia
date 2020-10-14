using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pudicitia.Common.Domain;
using Pudicitia.HR.Domain.Employees;

namespace Pudicitia.HR.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRContext context;
        private readonly DbSet<Employee> employees;

        public EmployeeRepository(HRContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            employees = context.Set<Employee>();
        }

        public async Task<ICollection<Employee>> GetEmployeesAsync(int offset, int limit)
        {
            var result = await employees
                .Include(x => x.JobChanges)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return result;
        }

        public async Task<ICollection<Employee>> GetEmployeesAsync(Guid departmentId, int offset, int limit)
        {
            var result = await employees
                .Where(x => x.DepartmentId == departmentId)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return result;
        }

        public async Task<Employee> GetEmployeeAsync(Guid employeeId)
        {
            var result = await employees
                .Include(x => x.JobChanges)
                .SingleOrDefaultAsync(x => x.Id == employeeId) ??
                throw new EntityNotFoundException(typeof(Employee), employeeId);

            return result;
        }

        public async Task<int> GetEmployeesCountAsync()
        {
            var result = await employees
                .CountAsync();

            return result;
        }

        public async Task<int> GetEmployeesCountAsync(Guid departmentId)
        {
            var result = await employees
                .CountAsync(x => x.DepartmentId == departmentId);

            return result;
        }

        public void Add(Employee employee)
        {
            employees.Add(employee);
        }

        public void Update(Employee employee)
        {
            employees.Update(employee);
        }
    }
}