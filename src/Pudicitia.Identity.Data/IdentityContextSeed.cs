using Pudicitia.Identity.Domain.Permissions;
using Pudicitia.Identity.Domain.Roles;
using Pudicitia.Identity.Domain.Users;

namespace Pudicitia.Identity.Data;

public class IdentityContextSeed
{
    private readonly IdentityContext _context;

    public IdentityContextSeed(IdentityContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task SeedAsync()
    {
        if (_context.Set<User>().Any())
        {
            return;
        }

        var users = GetUsers();
        var roles = GetRoles();
        var permissions = GetPermissions();

        foreach (var user in users)
        {
            foreach (var role in roles)
            {
                user.AssignRole(role);
            }
        }

        foreach (var role in roles)
        {
            foreach (var permission in permissions)
            {
                role.AssignPermission(permission);
            }
        }

        _context.Set<User>().AddRange(users);
        _context.Set<Role>().AddRange(roles);
        _context.Set<Permission>().AddRange(permissions);

        _context.LogEvents();
        await _context.SaveChangesAsync();
    }

    private IEnumerable<User> GetUsers()
    {
        var result = new User[]
        {
            new User("Admin", "123fff", "Admin", "Admin", true)
        };

        return result;
    }

    private IEnumerable<Role> GetRoles()
    {
        var result = new Role[]
        {
            new Role("Administrator", true),
            new Role("Human Resources", true)
        };

        return result;
    }

    private IEnumerable<Permission> GetPermissions()
    {
        var result = new Permission[]
        {
            new Permission("SignIn", "Sign In", string.Empty, true),
            new Permission("HumanResources", "Human Resources", string.Empty, true),
        };

        return result;
    }
}
