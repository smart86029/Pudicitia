namespace Pudicitia.Enterprise.Gateway.Models.HR;

public class GetEmployeesInput : PaginationOptions
{
    public Guid DepartmentId { get; set; }
}
