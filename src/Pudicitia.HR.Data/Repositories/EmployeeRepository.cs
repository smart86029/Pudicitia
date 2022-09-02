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
        var results = await _employees
            .Include(x => x.JobChanges)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        return results;
    }

    public async Task<ICollection<Employee>> GetEmployeesAsync(Guid departmentId, int offset, int limit)
    {
        var results = await _employees
            .Where(x => x.DepartmentId == departmentId)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        return results;
    }

    public async Task<Employee> GetEmployeeAsync(Guid employeeId)
    {
        var result = await _employees
            .Include(x => x.JobChanges)
            .SingleOrDefaultAsync(x => x.Id == employeeId)
            ?? throw new EntityNotFoundException<Employee>(employeeId);

        return result;
    }

    public async Task<int> GetCountAsync()
    {
        var result = await _employees
            .CountAsync();

        return result;
    }

    public async Task<int> GetCountByUserAsync(Guid userId)
    {
        var result = await _employees
            .CountAsync(x => x.UserId == userId);

        return result;
    }


    public async Task<int> GetCountByDepartmentAsync(Guid departmentId)
    {
        var result = await _employees
            .CountAsync(x => x.DepartmentId == departmentId);

        return result;
    }

    public async Task<int> GetCountByJobAsync(Guid jobId)
    {
        var result = await _employees
            .CountAsync(x => x.JobId == jobId);

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
