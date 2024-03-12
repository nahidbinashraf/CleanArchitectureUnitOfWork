using UnitOfWork.Core.Interfaces;
using UnitOfWork.Core.Models.Entities;
using UnitOfWork.Infrastructure.Data;

namespace UnitOfWork.Infrastructure.Repositories
{
    public class DepartmentRespository : GenericRepository<Department> , IDepartmentRespository
    {
        public DepartmentRespository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
