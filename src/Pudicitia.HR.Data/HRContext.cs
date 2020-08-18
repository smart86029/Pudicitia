using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Pudicitia.Common.Domain;
using Pudicitia.Common.EntityFrameworkCore.Configurations;
using Pudicitia.Common.EntityFrameworkCore.Converters;
using Pudicitia.HR.Data.Configurations;

namespace Pudicitia.HR.Data
{
    public class HRContext : DbContext
    {
        public HRContext(DbContextOptions<HRContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("HR");
            modelBuilder.Ignore<DomainEvent>();
            modelBuilder.ApplyConfiguration(new EventLogConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new JobTitleConfiguration());
            modelBuilder.ApplyConfiguration(new JobChangeConfiguration());

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
