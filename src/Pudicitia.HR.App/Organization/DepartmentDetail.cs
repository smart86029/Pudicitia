namespace Pudicitia.HR.App.Organization;

public class DepartmentDetail : EntityResult
{
    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public Guid? ParentId { get; set; }
}
