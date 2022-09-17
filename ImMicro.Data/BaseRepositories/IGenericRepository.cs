using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImMicro.Data.BaseRepositories
{
    public interface IGenericRepository<TEntity>
    {
        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken);
        Task<List<TEntity>> InsertManyAsync(List<TEntity> entityList, CancellationToken cancellationToken);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        Task<List<TEntity>> UpdateManyAsync(List<TEntity> entityList, CancellationToken cancellationToken);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
        Task DeleteManyAsync(List<TEntity> entityList, CancellationToken cancellationToken);


        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        Task<TEntity> FindOneWithAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> FilterBy(Expression<Func<TEntity, bool>> filterExpression);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        Task BulkInsertAsync(List<TEntity> entities, CancellationToken cancellationToken);
        void BulkInsert(List<TEntity> entities);
        Task BulkUpdateAsync(List<TEntity> entities, CancellationToken cancellationToken);
        void BulkUpdate(List<TEntity> entities);
    }
}