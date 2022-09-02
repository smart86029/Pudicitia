using System.Linq.Expressions;

namespace Pudicitia.HR.Domain.Employees
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<ICollection<Employee>> GetEmployeesAsync(int offset, int limit);

        Task<ICollection<Employee>> GetEmployeesAsync(Guid departmentId, int offset, int limit);

        Task<Employee> GetEmployeeAsync(Guid employeeId);

        Task<int> GetCountAsync();

        Task<int> GetCountByUserAsync(Guid userId);

        Task<int> GetCountByDepartmentAsync(Guid departmentId);

        Task<int> GetCountByJobAsync(Guid jobId);

        void Add(Employee employee);

        void Update(Employee employee);
    }
}
