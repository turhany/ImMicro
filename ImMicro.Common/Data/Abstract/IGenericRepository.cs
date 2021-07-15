using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ImMicro.Common.Data.Abstract
{
    public interface IGenericRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> FindFirstByAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> DeleteAsync(TEntity entity);
        
        Task BulkInsertAsync(IEnumerable<TEntity> entities);
        void BulkInsert(IEnumerable<TEntity> entities);
       
        TEntity CreateProxy();
    }
}