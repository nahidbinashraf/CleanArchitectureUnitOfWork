using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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


        public int Save()
        {
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
    }
}
