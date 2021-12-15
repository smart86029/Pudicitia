namespace Pudicitia.Common.Domain;

public interface IUnitOfWork
{
    Task<bool> CommitAsync();
}
