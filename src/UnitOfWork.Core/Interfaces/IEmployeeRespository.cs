using UnitOfWork.Core.Models.Entities;

namespace UnitOfWork.Core.Interfaces
{
    public interface IEmployeeRespository : IGenericRepository<Employee>
    {
        public int Count();
    }
}
