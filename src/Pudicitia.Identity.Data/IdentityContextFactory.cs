using Microsoft.EntityFrameworkCore.Design;

namespace Pudicitia.Identity.Data;

internal class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
{
    public IdentityContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=PudicitiaIdentity;User Id=sa;Password=Pass@word;MultipleActiveResultSets=True;");

        return new IdentityContext(optionsBuilder.Options);
    }
}
