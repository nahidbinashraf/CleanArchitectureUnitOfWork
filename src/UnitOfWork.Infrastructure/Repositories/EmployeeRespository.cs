using UnitOfWork.Core.Interfaces;
using UnitOfWork.Core.Models.Entities;
using UnitOfWork.Infrastructure.Data;

namespace UnitOfWork.Infrastructure.Repositories
{
    public class EmployeeRespository : GenericRepository<Employee> ,IEmployeeRespository
    {
        public EmployeeRespository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public int Count()
        {
            return _context.Employees.Count();
        }
    }
}
