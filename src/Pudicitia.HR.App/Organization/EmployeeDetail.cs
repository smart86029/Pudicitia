using Pudicitia.HR.Domain;

namespace Pudicitia.HR.App.Organization;

public class EmployeeDetail : EntityResult
{
    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }

    public Gender Gender { get; set; }

    public MaritalStatus MaritalStatus { get; set; }

    public Guid DepartmentId { get; set; }

    public Guid JobId { get; set; }
}
