namespace UnitOfWork.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRespository EmployeeRespository { get; }
        IDepartmentRespository DepartmentRespository { get; }

        void CreateTransaction();

        void Commit();

        void Rollback();

        int Save(bool includeAuditLog = true);

        string GetDatabaseProvider();
    }
}
