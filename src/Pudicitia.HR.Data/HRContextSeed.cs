using Pudicitia.HR.Domain;
using Pudicitia.HR.Domain.Departments;
using Pudicitia.HR.Domain.Employees;
using Pudicitia.HR.Domain.Jobs;
using Pudicitia.HR.Domain.Leaves;

namespace Pudicitia.HR.Data;

public sealed class HRContextSeed
{
    private readonly HRContext _context;

    public HRContextSeed(HRContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task SeedAsync()
    {
        if (_context.Set<Department>().Any())
        {
            return;
        }

        var employees = GetEmployees();
        var departments = GetDepartments();
        var jobs = GetJobs();
        var leaves = GetLeaves(employees);

        employees[0].AssignJob(departments[0], jobs[0], true, DateTime.Parse("2018-07-01"));
        employees[1].AssignJob(departments[1], jobs[1], true, DateTime.Parse("2018-07-01"));
        employees[2].AssignJob(departments[2], jobs[2], true, DateTime.Parse("2018-07-01"));
        employees[3].AssignJob(departments[3], jobs[2], true, DateTime.Parse("2018-07-15"));
        employees[4].AssignJob(departments[4], jobs[2], true, DateTime.Parse("2018-08-04"));
        employees[5].AssignJob(departments[1], jobs[3], false, DateTime.Parse("2018-08-04"));
        employees[6].AssignJob(departments[2], jobs[6], false, DateTime.Parse("2018-08-04"));
        employees[7].AssignJob(departments[3], jobs[4], false, DateTime.Parse("2018-08-04"));
        employees[8].AssignJob(departments[3], jobs[4], false, DateTime.Parse("2018-08-04"));
        employees[9].AssignJob(departments[3], jobs[4], false, DateTime.Parse("2018-08-04"));
        employees[10].AssignJob(departments[4], jobs[5], false, DateTime.Parse("2018-08-04"));
        employees[11].AssignJob(departments[4], jobs[5], false, DateTime.Parse("2018-08-04"));

        _context.Set<Employee>().AddRange(employees);
        _context.Set<Department>().AddRange(departments);
        _context.Set<Job>().AddRange(jobs);
        _context.Set<Leave>().AddRange(leaves);

        _context.LogEvents();
        await _context.SaveChangesAsync();
    }

    private List<Employee> GetEmployees()
    {
        var results = new List<Employee>
        {
            new Employee("William Glaze", "William", DateTime.Parse("1955-12-02"), Gender.Male, MaritalStatus.Married),
            new Employee("Kelley Hennig", "Kelley", DateTime.Parse("1984-01-02"), Gender.Female, MaritalStatus.Married),
            new Employee("Raymond Miller", "Raymond", DateTime.Parse("1981-11-05"), Gender.Male, MaritalStatus.Married),
            new Employee("Zella Rogers", "Zella", DateTime.Parse("1981-11-24"), Gender.Female, MaritalStatus.Married),
            new Employee("Joel Metcalfe", "Joel", DateTime.Parse("1985-05-12"), Gender.Male, MaritalStatus.Single),
            new Employee("Anita Bowles", "Anita", DateTime.Parse("1992-08-27"), Gender.Female, MaritalStatus.Single),
            new Employee("Ben Buendia", "Ben", DateTime.Parse("1983-04-16"), Gender.Male, MaritalStatus.Single),
            new Employee("Kian Marsh", "Kian", DateTime.Parse("1987-03-19"), Gender.Male, MaritalStatus.Single),
            new Employee("Nancy Morrison", "Nancy", DateTime.Parse("1984-08-09"), Gender.Female, MaritalStatus.Single),
            new Employee("Riley Hooper", "Riley", DateTime.Parse("1996-11-24"), Gender.Male, MaritalStatus.Single),
            new Employee("Jonathan Abbott", "Jonathan", DateTime.Parse("1992-02-13"), Gender.Male, MaritalStatus.Single),
            new Employee("Gina Barnes", "Gina", DateTime.Parse("1986-10-20"), Gender.Female, MaritalStatus.Married),
        };

        return results;
    }

    private List<Department> GetDepartments()
    {
        var results = new List<Department>();
        var departmentRoot = new Department("Headquarters", true, null);
        results.Add(departmentRoot);
        results.Add(new Department("Finance", true, departmentRoot.Id));
        results.Add(new Department("Human Resources", true, departmentRoot.Id));
        results.Add(new Department("Research & Development", true, departmentRoot.Id));
        results.Add(new Department("Information Technology", true, departmentRoot.Id));

        return results;
    }

    private List<Job> GetJobs()
    {
        var results = new List<Job>
        {
            new Job("Chief Executive Officer", true),
            new Job("Chief Financial Officer", true),
            new Job("Manager", true),
            new Job("Accountant", true),
            new Job("Engineer", true),
            new Job("Technician", true),
            new Job("Staff", true),
        };

        return results;
    }

    private List<Leave> GetLeaves(List<Employee> employees)
    {
        var results = new List<Leave>
        {
            new Leave(LeaveType.Sick, DateTime.Parse("2022-04-20 09:00:00"), DateTime.Parse("2022-04-20 18:00:00"), string.Empty, employees[2].Id),
            new Leave(LeaveType.Compensatory, DateTime.Parse("2022-04-22 09:00:00"), DateTime.Parse("2022-04-22 18:00:00"), string.Empty, employees[3].Id),
        };

        return results;
    }
}
