namespace Pudicitia.HR.App.Organization;

public class EmployeeOptions : PaginationOptions
{
    public string? Name { get; set; }

    public Guid? DepartmentId { get; set; }
}
