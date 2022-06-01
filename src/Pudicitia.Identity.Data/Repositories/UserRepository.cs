using Pudicitia.Identity.Domain.Users;

namespace Pudicitia.Identity.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IdentityContext _context;
    private readonly DbSet<User> _users;

    public UserRepository(IdentityContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _users = context.Set<User>();
    }

    public async Task<ICollection<User>> GetUsersAsync(int offset, int limit)
    {
        var results = await _users
            .Include(x => x.UserRoles)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        return results;
    }

    public async Task<User> GetUserAsync(Guid userId)
    {
        var result = await _users
            .Include(x => x.UserRoles)
            .Include(x => x.UserRefreshTokens)
            .SingleOrDefaultAsync(x => x.Id == userId)
            ?? throw new EntityNotFoundException<User>(userId);

        return result;
    }

    public async Task<User> GetUserAsync(string userName, string password)
    {
        var result = await _users
            .Include(x => x.UserRoles)
            .Include(x => x.UserRefreshTokens)
            .SingleOrDefaultAsync(x => x.UserName == userName)
            ?? throw new EntityNotFoundException<User>();
        if (result.PasswordHash != CryptographyUtility.Hash(password.Trim(), result.Salt))
        {
            throw new EntityNotFoundException<User>();
        }

        return result;
    }

    public async Task<int> GetCountAsync()
    {
        var result = await _users
            .CountAsync();

        return result;
    }

    public void Add(User user)
    {
        _users.Add(user);
    }

    public void Update(User user)
    {
        _users.Update(user);
    }

    public void Remove(User user)
    {
        _users.Remove(user);
    }
}
