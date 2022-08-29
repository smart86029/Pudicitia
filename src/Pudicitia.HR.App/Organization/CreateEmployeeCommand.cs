namespace Pudicitia.HR.App.Organization;

public class CreateEmployeeCommand
{
    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }

    public Gender Gender { get; set; }

    public MaritalStatus MaritalStatus { get; set; }
}
