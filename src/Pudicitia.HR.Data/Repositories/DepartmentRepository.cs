using Pudicitia.HR.Domain.Departments;

namespace Pudicitia.HR.Data.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly HRContext _context;
    private readonly DbSet<Department> _departments;

    public DepartmentRepository(HRContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _departments = context.Set<Department>();
    }

    public async Task<ICollection<Department>> GetDepartmentsAsync()
    {
        var results = await _departments
            .ToListAsync();

        return results;
    }

    public async Task<Department> GetDepartmentAsync(Guid departmentId)
    {
        var result = await _departments
            .SingleOrDefaultAsync(x => x.Id == departmentId)
            ?? throw new EntityNotFoundException<Department>(departmentId);

        return result;
    }

    public void Add(Department department)
    {
        _departments.Add(department);
    }

    public void Update(Department department)
    {
        _departments.Update(department);
    }

    public void Remove(Department department)
    {
        _departments.Remove(department);
    }
}
