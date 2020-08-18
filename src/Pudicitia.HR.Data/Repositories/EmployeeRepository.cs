using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pudicitia.HR.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Pudicitia.Common.Domain;

namespace Pudicitia.HR.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRContext context;

   
        public EmployeeRepository(HRContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<Employee>> GetEmployeesAsync(int offset, int limit)
        {
            var result = await context
                .Set<Employee>()
                .Include(e => e.JobChanges)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return result;
        }

        public async Task<Employee> GetEmployeeAsync(Guid employeeId)
        {
            var result = await context
                .Set<Employee>()
                .Include(e => e.JobChanges)
                .SingleOrDefaultAsync(e => e.Id == employeeId) ??
                throw new EntityNotFoundException(typeof(Employee), employeeId);

            return result;
        }

        public Task<int> GetEmployeesCountAsync()
        {
            return context.Set<Employee>().CountAsync();
        }

        public void Add(Employee employee)
        {
            context.Set<Employee>().Add(employee);
        }

        public void Update(Employee employee)
        {
            context.Set<Employee>().Update(employee);
        }
    }
}