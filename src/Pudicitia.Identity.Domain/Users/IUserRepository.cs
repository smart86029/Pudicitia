namespace Pudicitia.Identity.Domain.Users;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetUserAsync(Guid userId);

    Task<User> GetUserAsync(string userName, string password);

    Task<bool> ContainsAsync(Guid userId);

    void Add(User user);

    void Update(User user);

    void Remove(User user);
}
