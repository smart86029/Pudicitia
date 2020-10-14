using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pudicitia.Common.Domain;
using Pudicitia.HR.Domain.Departments;

namespace Pudicitia.HR.Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HRContext context;
        private readonly DbSet<Department> departments;

        public DepartmentRepository(HRContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            departments = context.Set<Department>();
        }

        public async Task<ICollection<Department>> GetDepartmentsAsync()
        {
            var result = await departments
                .ToListAsync();

            return result;
        }

        public async Task<Department> GetDepartmentAsync(Guid departmentId)
        {
            var result = await departments
                .SingleOrDefaultAsync(x => x.Id == departmentId) ??
                throw new EntityNotFoundException(typeof(Department), departmentId);

            return result;
        }

        public void Add(Department department)
        {
            departments.Add(department);
        }

        public void Update(Department department)
        {
            departments.Update(department);
        }

        public void Remove(Department department)
        {
            departments.Remove(department);
        }
    }
}