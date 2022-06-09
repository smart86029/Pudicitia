namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class GetEmployeesInput : PaginationOptions
{
    public Guid DepartmentId { get; set; }
}
