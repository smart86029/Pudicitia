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
        var password = "123fff";
        var result = new User[]
        {
            new User("admin", password, "Admin", "Admin", true),
            new User("william", password, "William Glaze", "William", true),
            new User("kelley", password, "Kelley Hennig", "Kelley", true),
            new User("raymond", password, "Raymond Miller", "Raymond", true),
            new User("zella", password, "Zella Rogers", "Zella", true),
            new User("joel", password, "Joel Metcalfe", "Joel", true),
            new User("anita", password, "Anita Bowles", "Anita", true),
            new User("ben", password, "Ben Buendia", "Ben", true),
            new User("kian", password, "Kian Marsh", "Kian", true),
            new User("nancy", password, "Nancy Morrison", "Nancy", true),
            new User("riley", password, "Riley Hooper", "Riley", true),
            new User("jonathan", password, "Jonathan Abbott", "Jonathan", true),
            new User("gina", password, "Gina Barnes", "Gina", true),
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
