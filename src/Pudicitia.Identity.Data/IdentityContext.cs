using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pudicitia.Common.Domain;
using Pudicitia.Common.EntityFrameworkCore.Configurations;
using Pudicitia.Common.EntityFrameworkCore.Converters;
using Pudicitia.Identity.Data.Configurations;

namespace Pudicitia.Identity.Data
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Identity");
            modelBuilder.Ignore<DomainEvent>();
            modelBuilder.ApplyConfiguration(new EventLogConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());

            var utcDateTimeConverter = new UtcDateTimeConverter();
            var dateTimeProperties = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(x => x.GetProperties())
                .Where(x => x.ClrType == typeof(DateTime));

            foreach (var property in dateTimeProperties)
                property.SetValueConverter(utcDateTimeConverter);
        }
    }
}