﻿using System;
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
            modelBuilder
                .HasDefaultSchema("Identity")
                .Ignore<DomainEvent>()
                .ApplyConfiguration(new EventPublishedConfiguration())
                .ApplyConfiguration(new EventSubscribedConfiguration())
                .ApplyConfiguration(new UserConfiguration())
                .ApplyConfiguration(new RoleConfiguration())
                .ApplyConfiguration(new PermissionConfiguration())
                .ApplyConfiguration(new UserRoleConfiguration())
                .ApplyConfiguration(new UserRefreshTokenConfiguration())
                .ApplyConfiguration(new RolePermissionConfiguration());

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