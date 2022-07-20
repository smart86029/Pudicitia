namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class GetEmployeesInput : Pagination
{
    public Guid DepartmentId { get; set; }
}
