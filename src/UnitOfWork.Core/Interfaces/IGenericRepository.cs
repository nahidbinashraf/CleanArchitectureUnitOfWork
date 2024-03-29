﻿using System.Linq.Expressions;
using UnitOfWork.Core.Models;

namespace UnitOfWork.Core.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class 
    {
        Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null, bool trackChanges = false);

        Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null, bool trackChanges = false);

        IList<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null, bool trackChanges = false);

        IPagedList<TEntity> GetAllPaged(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool trackChanges = false);

        Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool trackChanges = false);

        IQueryable<TEntity> FindAll(bool trackChanges = false);

        IQueryable<TEntity> FindByCondition(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null, bool trackChanges = false);

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
