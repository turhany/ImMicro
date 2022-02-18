using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ImMicro.Common.Data.Abstract
{
    public interface IGenericRepository<TEntity>
    {
        Task<TEntity> InsertAsync(TEntity entity);
        Task<List<TEntity>> InsertManyAsync(List<TEntity> entityList);
        
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<List<TEntity>> UpdateManyAsync(List<TEntity> entityList);

        Task DeleteAsync(TEntity entity);
        Task DeleteManyAsync(List<TEntity> entityList);

        
        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindOneWithAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        
        Task BulkInsertAsync(IEnumerable<TEntity> entities);
        void BulkInsert(IEnumerable<TEntity> entities); 
        Task BulkUpdateAsync(IEnumerable<TEntity> entities);
        void BulkUpdate(IEnumerable<TEntity> entities); 
    }
}