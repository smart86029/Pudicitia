namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class EmployeeDetail : EntityResult
{
    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }

    public int Gender { get; set; }

    public int MaritalStatus { get; set; }

    public Guid? UserId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;
}
