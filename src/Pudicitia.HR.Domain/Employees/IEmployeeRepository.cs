namespace Pudicitia.HR.Domain.Employees
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<ICollection<Employee>> GetEmployeesAsync(int offset, int limit);

        Task<ICollection<Employee>> GetEmployeesAsync(Guid departmentId, int offset, int limit);

        Task<Employee> GetEmployeeAsync(Guid employeeId);

        Task<int> GetEmployeesCountAsync();

        Task<int> GetEmployeesCountAsync(Guid departmentId);

        void Add(Employee employee);

        void Update(Employee employee);
    }
}
