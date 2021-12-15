namespace Pudicitia.HR.App.Organization;

public class UpdateDepartmentCommand : Command
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
}
