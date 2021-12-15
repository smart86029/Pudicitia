namespace Pudicitia.HR.App.Organization;

public class EmployeeSummary : EntityResult
{
    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public Guid DepartmentId { get; set; }

    public Guid JobId { get; set; }
}
