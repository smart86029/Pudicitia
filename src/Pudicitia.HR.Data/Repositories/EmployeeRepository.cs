using Pudicitia.HR.Domain.Employees;

namespace Pudicitia.HR.Data.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly HRContext _context;
    private readonly DbSet<Employee> _employees;

    public EmployeeRepository(HRContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _employees = context.Set<Employee>();
    }

    public async Task<ICollection<Employee>> GetEmployeesAsync(int offset, int limit)
    {
        var result = await _employees
            .Include(x => x.JobChanges)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        return result;
    }

    public async Task<ICollection<Employee>> GetEmployeesAsync(Guid departmentId, int offset, int limit)
    {
        var result = await _employees
            .Where(x => x.DepartmentId == departmentId)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        return result;
    }

    public async Task<Employee> GetEmployeeAsync(Guid employeeId)
    {
        var result = await _employees
            .Include(x => x.JobChanges)
            .SingleOrDefaultAsync(x => x.Id == employeeId)
            ?? throw new EntityNotFoundException(typeof(Employee), employeeId);

        return result;
    }

    public async Task<int> GetEmployeesCountAsync()
    {
        var result = await _employees
            .CountAsync();

        return result;
    }

    public async Task<int> GetEmployeesCountAsync(Guid departmentId)
    {
        var result = await _employees
            .CountAsync(x => x.DepartmentId == departmentId);

        return result;
    }

    public void Add(Employee employee)
    {
        _employees.Add(employee);
    }

    public void Update(Employee employee)
    {
        _employees.Update(employee);
    }
}
