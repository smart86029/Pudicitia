namespace Pudicitia.Common.Domain;

public class EntityNotFoundException<TEntity> : Exception
    where TEntity : Entity
{
    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string message)
        : base(message)
    {
    }

    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public EntityNotFoundException(Guid id)
        : this($"There is no such an entity. Entity type: {typeof(TEntity).FullName}, id: {id}")
    {
    }
}
