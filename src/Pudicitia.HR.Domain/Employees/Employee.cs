using Pudicitia.HR.Domain.Departments;
using Pudicitia.HR.Domain.Jobs;

namespace Pudicitia.HR.Domain.Employees;

public class Employee : Person
{
    private readonly List<JobChange> _jobChanges = new();

    private Employee()
    {
    }

    public Employee(string name, string displayName, DateTime birthDate, Gender gender, MaritalStatus maritalStatus)
        : base(name, displayName, birthDate, gender, maritalStatus)
    {
        RaiseDomainEvent(new EmployeeCreated(Id));
    }

    private JobChange LastJobChange =>
        _jobChanges.SingleOrDefault(x => DateTime.UtcNow.IsBetween(x.StartOn, x.EndOn)) ?? _jobChanges.Last();

    public Guid DepartmentId { get; private set; }

    public Guid JobId { get; private set; }

    public bool IsEmployed => _jobChanges.Any(x => DateTime.UtcNow.IsBetween(x.StartOn, x.EndOn));

    public IReadOnlyCollection<JobChange> JobChanges => _jobChanges.AsReadOnly();

    public void AssignJob(Department department, Job job, bool isHead, DateTime startOn)
    {
        _jobChanges.Add(new JobChange(Id, department.Id, job.Id, isHead, startOn));
        DepartmentId = department.Id;
        JobId = job.Id;
    }
}
