namespace Pudicitia.HR.Domain.Departments;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<ICollection<Department>> GetDepartmentsAsync();

    Task<Department> GetDepartmentAsync(Guid departmentId);

    void Add(Department department);

    void Update(Department department);

    void Remove(Department department);
}
