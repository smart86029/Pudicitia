using Microsoft.EntityFrameworkCore.Design;

namespace Pudicitia.HR.Data;

internal class HRContextFactory : IDesignTimeDbContextFactory<HRContext>
{
    public HRContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<HRContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=PudicitiaHR;User Id=sa;Password=Pass@word;MultipleActiveResultSets=True;");

        return new HRContext(optionsBuilder.Options);
    }
}
