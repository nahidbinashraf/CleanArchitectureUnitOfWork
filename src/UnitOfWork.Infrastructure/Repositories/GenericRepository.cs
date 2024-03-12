using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.PortableExecutable;
using UnitOfWork.Core.Extension;
using UnitOfWork.Core.Interfaces;
using UnitOfWork.Core.Models;
using UnitOfWork.Infrastructure.Data;

namespace UnitOfWork.Infrastructure.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity?> GetByIdAsync(long id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public TEntity? GetById(long id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public async Task<IList<TEntity>> GetByIdsAsync(IList<long> ids)
        {
            return await _context.Set<TEntity>()
                .Where(entity => ids.Contains((long)_context.Entry(entity).Property("Id").CurrentValue!))
                .ToListAsync();
        }

        public async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null, bool trackChanges = false)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            query = func != null ? func(query) : query;

            return await query.ToListAsync();
        }

        public async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null, bool trackChanges = false)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            query = func != null ? await func(query) : query;

            return await query.ToListAsync();
        }

        public IList<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null, bool trackChanges = false)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            query = func != null ? func(query) : query;

            return query.ToList();
        }

        public IPagedList<TEntity> GetAllPaged(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool trackChanges = false)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            query = func != null ? func(query) : query;

            return query.ToPagedList(pageIndex, pageSize, getOnlyTotalCount);
        }

        public async Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool trackChanges = false)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            query = func != null ? await func(query) : query;

            return query.ToPagedList(pageIndex, pageSize, getOnlyTotalCount);
        }

        public IQueryable<TEntity> FindAll(bool trackChanges = false)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public async Task<IList<TModel>> RawSqlQueryAsync<TModel>(string sqlQuery, params object[] parameters)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = sqlQuery;

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    var dbParameter = command.CreateParameter();
                    dbParameter.Value = parameter;
                    command.Parameters.Add(dbParameter);
                }
            }

            _context.Database.OpenConnection();

            try
            {
                using var result = await command.ExecuteReaderAsync();
                return MapResult<TModel>(result);
            }
            finally
            {
                _context.Database.CloseConnection();
            }
        }

        public IList<TModel> RawSqlQuery<TModel>(string sqlQuery, params object[] parameters)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = sqlQuery;

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    var dbParameter = command.CreateParameter();
                    dbParameter.Value = parameter;
                    command.Parameters.Add(dbParameter);
                }
            }

            _context.Database.OpenConnection();

            try
            {
                using var result = command.ExecuteReader();
                return MapResult<TModel>(result);
            }
            finally
            {
                _context.Database.CloseConnection();
            }
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public void Update(TEntity entity, params Expression<Func<TEntity, object>>[] propertiesToUpdate)
        {
            if (propertiesToUpdate == null || propertiesToUpdate.Length == 0)
            {
                _context.Set<TEntity>().Update(entity);
            }
            else
            {
                var entry = _context.Entry(entity);

                if (entry.State == EntityState.Detached)
                {
                    _context.Set<TEntity>().Attach(entity);
                }

                foreach (var property in propertiesToUpdate)
                {
                    entry.Property(property).IsModified = true;
                }
            }
        }

        public void UpdateRange(IEnumerable<TEntity> entities, params Expression<Func<TEntity, object>>[] propertiesToUpdate)
        {
            if (propertiesToUpdate == null || propertiesToUpdate.Length == 0)
            {
                _context.Set<TEntity>().UpdateRange(entities);
            }
            else
            {
                foreach (var entity in entities)
                {
                    var entry = _context.Entry(entity);

                    if (entry.State == EntityState.Detached)
                    {
                        _context.Set<TEntity>().Attach(entity);
                    }

                    foreach (var property in propertiesToUpdate)
                    {
                        entry.Property(property).IsModified = true;
                    }
                }
            }
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        private static IList<TModel> MapResult<TModel>(DbDataReader reader)
        {
            var results = new List<TModel>();

            while (reader.Read())
            {
                var model = Activator.CreateInstance<TModel>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var propertyName = reader.GetName(i);
                    var property = typeof(TModel).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (property != null && property.CanWrite && !reader.IsDBNull(i))
                    {
                        property.SetValue(model, reader.GetValue(i));
                    }
                }

                results.Add(model);
            }

            return results;
        }
    }
}
