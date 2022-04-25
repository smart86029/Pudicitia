namespace Pudicitia.Identity.Events.HR;

public class EmployeeCreated : Event
{
    public Guid EmployeeId { get; set; }
}
