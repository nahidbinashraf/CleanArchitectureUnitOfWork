using System.Linq.Expressions;

namespace UnitOfWork.Core.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class 
    {
        Task<TEntity?> GetByIdAsync(long id);

        TEntity? GetById(long id);

        Task<IList<TEntity>> GetByIdsAsync(IList<long> ids);

        Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null, bool trackChanges = false);

        Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null, bool trackChanges = false);

        IList<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null, bool trackChanges = false);
     
        IQueryable<TEntity> FindAll(bool trackChanges = false);

        Task<IList<TModel>> RawSqlQueryAsync<TModel>(string sqlQuery, params object[] parameters);

        IList<TModel> RawSqlQuery<TModel>(string sqlQuery, params object[] parameters);

        Task AddAsync(TEntity entity);

        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity, params Expression<Func<TEntity, object>>[] propertiesToUpdate);

        void UpdateRange(IEnumerable<TEntity> entities, params Expression<Func<TEntity, object>>[] propertiesToUpdate);

        void Delete(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> entities);
    }
}
