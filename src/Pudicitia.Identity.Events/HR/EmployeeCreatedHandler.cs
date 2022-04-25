namespace Pudicitia.Identity.Events.HR;

public class EmployeeCreatedHandler : IEventHandler<EmployeeCreated>
{
    private readonly IIdentityUnitOfWork unitOfWork;

    public EmployeeCreatedHandler(IIdentityUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(EmployeeCreated @event)
    {
        Console.WriteLine("Handle" + DateTime.Now);
        await unitOfWork.CommitAsync();
    }
}
