namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class CreateEmployeeInput
{
    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }

    public int Gender { get; set; }

    public int MaritalStatus { get; set; }
}
