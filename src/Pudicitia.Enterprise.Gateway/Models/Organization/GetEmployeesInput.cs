namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class GetEmployeesInput : Pagination
{
    public string? Name { get; set; }

    public Guid? DepartmentId { get; set; }
}
