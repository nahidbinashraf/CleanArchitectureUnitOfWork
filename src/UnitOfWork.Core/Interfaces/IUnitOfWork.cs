namespace UnitOfWork.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRespository EmployeeRespository { get; }
        int Save();
    }
}
