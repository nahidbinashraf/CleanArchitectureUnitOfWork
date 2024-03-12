using UnitOfWork.Core.Interfaces;
using UnitOfWork.Infrastructure.Data;

namespace UnitOfWork.Infrastructure.Repositories
{
    public class UnitOfWorks : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public IEmployeeRespository  EmployeeRespository{ get; }

        public UnitOfWorks(ApplicationDbContext dbContext,
            IEmployeeRespository employeeRespository)
        {
            _dbContext = dbContext;
            EmployeeRespository = employeeRespository;
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
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
