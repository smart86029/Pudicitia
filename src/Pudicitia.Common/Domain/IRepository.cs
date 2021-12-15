namespace Pudicitia.Common.Domain;

public interface IRepository<TAggregateRoot>
    where TAggregateRoot : AggregateRoot
{
}
