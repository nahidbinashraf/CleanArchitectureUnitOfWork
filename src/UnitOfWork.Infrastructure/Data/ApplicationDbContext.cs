using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UnitOfWork.Core;
using UnitOfWork.Core.Models.Entities;

namespace UnitOfWork.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions) : base(contextOptions)
        {

        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Designation> Designations { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        public override int SaveChanges()
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added
                || e.State == EntityState.Modified
                || e.State == EntityState.Deleted)
                .ToList();
            foreach (var modifiedEntity in modifiedEntities)
            {
                var auditLog = new AuditLog
                {
                    TableName = modifiedEntity.Entity.GetType().Name,
                    Action = modifiedEntity.State.ToString(),
                    Timestamp = DateTime.UtcNow,
                    UserInfo = "Test",
                    NewValue = GetModifiedValues(modifiedEntity),
                    OriginalValue = GetOriginalValues(modifiedEntity)
                };
                AuditLogs.Add(auditLog);
            }
            return base.SaveChanges();
        }

        private static string GetOriginalValues(EntityEntry modifiedEntity)
        {
            var originalValues = modifiedEntity.OriginalValues.Properties
                .Where(p => modifiedEntity.OriginalValues[p] != null)
                .Select(p => $"{p.Name}: {modifiedEntity.OriginalValues[p]}")
                .ToList();

            return originalValues.Any() ? string.Join(", ", originalValues) : "No original values";
        }

        private static string GetModifiedValues(EntityEntry modifiedEntity)
        {
            var modifiedValues = modifiedEntity.Properties
                .Where(p => p.IsModified && p.CurrentValue != null)
                .Select(p => $"{p.Metadata.Name}: {p.CurrentValue}")
                .ToList();

            return modifiedValues.Any() ? string.Join(", ", modifiedValues) : "No modified values";
        }
    }
}
