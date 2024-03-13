using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using UnitOfWork.Core;
using UnitOfWork.Core.Interfaces;
using UnitOfWork.Infrastructure.Data;

namespace UnitOfWork.Infrastructure.Repositories
{
    public class UnitOfWorks : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private IDbContextTransaction? _objTran = null;
        private readonly Lazy<IEmployeeRespository> _employeeRespository;
        private readonly Lazy<IDepartmentRespository> _departmentRespository;

        public UnitOfWorks(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            _employeeRespository = new Lazy<IEmployeeRespository>(() =>
                new EmployeeRespository(_dbContext));
            _departmentRespository = new Lazy<IDepartmentRespository>(() =>
            new DepartmentRespository(_dbContext));
        }

        public IEmployeeRespository EmployeeRespository => _employeeRespository.Value;
        public IDepartmentRespository DepartmentRespository => _departmentRespository.Value;

        public void CreateTransaction()
        {
            _objTran = _dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            _objTran?.Commit();
        }

        public void Rollback()
        {
            _objTran?.Rollback();
            _objTran?.Dispose();
        }


        public int Save(bool includeAuditLog = true)
        {
            if (includeAuditLog)
            {
                var modifiedEntities = _dbContext.ChangeTracker.Entries()
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
                    _dbContext.AuditLogs.Add(auditLog);
                }
            }
            return _dbContext.SaveChanges();
        }

        public string GetDatabaseProvider()
        {
            return _dbContext?.Database?.IsSqlServer() == true ? "SqlServer" : "";
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
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
