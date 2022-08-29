namespace Pudicitia.HR.App.Organization;

public class UpdateEmployeeCommand : Command
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }

    public Gender Gender { get; set; }

    public MaritalStatus MaritalStatus { get; set; }
}
