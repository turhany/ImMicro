using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ImMicro.Common.Data;
using ImMicro.Common.Data.Abstract;

namespace ImMicro.Data.Repositories
{
    public class EfGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
    {
        private readonly DataContext _context;
        private readonly DbSet<TEntity> _entities;

        public EfGenericRepository(DataContext context)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
        }
        
        public async Task<TEntity> FindFirstByAsync(Expression<Func<TEntity, bool>> predicate) => await _entities.FirstOrDefaultAsync(predicate);
        
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var entityEntry = await _entities.AddAsync(entity);
            await _context.SaveEntitiesAsync();
            if (entityEntry.Entity.Id != default)
            {
                return entityEntry.Entity;
            }

            return default;
        }
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var entityEntry = _entities.Update(entity);
            await _context.SaveEntitiesAsync();
            return entityEntry.Entity;
        }
        
        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            var entityEntry = _entities.Remove(entity);
            await _context.SaveEntitiesAsync();
            return entityEntry.Entity;
        }
        
        public TEntity CreateProxy() => _entities.CreateProxy();
    }
}