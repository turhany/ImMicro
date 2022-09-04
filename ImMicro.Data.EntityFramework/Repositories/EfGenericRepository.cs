using System.Linq.Expressions;
using Audity.Extensions;
using Audity.Model;
using ImMicro.Common.Application;
using Microsoft.EntityFrameworkCore;
using ImMicro.Model.AuditLog;
using Newtonsoft.Json;
using Npgsql.Bulk;
using Throw;
using ImMicro.Data.BaseRepositories;
using ImMicro.Data.BaseModels;

namespace ImMicro.Data.EntityFramework.Repositories
{
    public class EfGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DataContext _context;
        private readonly DbSet<TEntity> _entities;
        private readonly DbSet<AuditLog> _auditLog;

        public EfGenericRepository(DataContext dbContext)
        {
            _context = dbContext;
            _entities = _context.Set<TEntity>();
            _auditLog = _context.Set<AuditLog>();
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.ThrowIfNull();
            
            await _entities.AddAsync(entity, cancellationToken);
            await SaveAuditLogsAsync(cancellationToken);
            await _context.SaveEntitiesAsync(cancellationToken);
            _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task<List<TEntity>> InsertManyAsync(List<TEntity> entityList, CancellationToken cancellationToken)
        {
            entityList.ThrowIfNull();
            entityList.Throw().IfEmpty();
             
            await _entities.AddRangeAsync(entityList, cancellationToken);
            await SaveAuditLogsAsync(cancellationToken);
            await _context.SaveEntitiesAsync(cancellationToken);

            foreach (var entity in entityList)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entityList;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.ThrowIfNull();
            
            await SaveAuditLogsAsync(cancellationToken);
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveEntitiesAsync(cancellationToken);
            _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task<List<TEntity>> UpdateManyAsync(List<TEntity> entityList, CancellationToken cancellationToken)
        {
            entityList.ThrowIfNull();
            entityList.Throw().IfEmpty();

            await SaveAuditLogsAsync(cancellationToken);
            _entities.UpdateRange(entityList);
            await _context.SaveEntitiesAsync(cancellationToken);
            foreach (var entity in entityList)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entityList;
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.ThrowIfNull();
            
            if (entity is SoftDeleteEntity softDeleteEntity)
            {
                SetDeleteFields(softDeleteEntity); 
                await UpdateAsync(entity, cancellationToken);
            }
            else
            {
                _entities.Remove(entity);
                await SaveAuditLogsAsync(cancellationToken);
                await _context.SaveEntitiesAsync(cancellationToken);
            }
        }

        public async Task DeleteManyAsync(List<TEntity> entityList, CancellationToken cancellationToken)
        {
            entityList.ThrowIfNull();
            entityList.Throw().IfEmpty();

            if (entityList.First() is SoftDeleteEntity)
            {
                foreach (var entity in entityList)
                {
                    SetDeleteFields(entity as SoftDeleteEntity);
                }

                await UpdateManyAsync(entityList, cancellationToken);
            }
            else
            {
                _entities.RemoveRange(entityList);
                await SaveAuditLogsAsync(cancellationToken);
                await _context.SaveEntitiesAsync(cancellationToken);
            }
        }

        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) 
            => await _entities.FirstOrDefaultAsync(predicate, cancellationToken);

        public async Task<TEntity> FindOneWithAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) 
            => await _entities.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) 
            => _entities.Where(predicate);

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) 
            => await _entities.AnyAsync(predicate, cancellationToken);

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) 
            => await _entities.CountAsync(predicate, cancellationToken);

        public async Task BulkInsertAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            var bulkUploader = new NpgsqlBulkUploader(_context);
            await bulkUploader.InsertAsync(entities);
        }
        
        public void BulkInsert(List<TEntity> entities)
        {
            var bulkUploader = new NpgsqlBulkUploader(_context);
            bulkUploader.Insert(entities);
        }

        public async Task BulkUpdateAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            var bulkUploader = new NpgsqlBulkUploader(_context);
            await bulkUploader.UpdateAsync(entities);
        }

        public void BulkUpdate(List<TEntity> entities)
        {
            var bulkUploader = new NpgsqlBulkUploader(_context);
            bulkUploader.Update(entities);
        }

        #region Private Methods

        private void SetDeleteFields(SoftDeleteEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = ApplicationContext.Instance.CurrentUser.Id;
        }

        private async Task SaveAuditLogsAsync(CancellationToken cancellationToken)
        {
            var auditResponse = _context.ChangeTracker.GetAuditData(new AuditConfiguration
            {
                KeyPropertyName = "Id",
                ExcludeEntities = new List<string> { "RequestLog" },
                MaskedProperties = new List<string>{ "Password" }
            });
            
            if (auditResponse.AuditLogEntries.Any())
            {
                var auditLogs = new List<AuditLog>();
                var serializedEnvironmentData = auditResponse.EnvironmentData != null ? JsonConvert.SerializeObject(auditResponse.EnvironmentData) : null;
                
                foreach (var auditLogEntry in auditResponse.AuditLogEntries)
                {
                    auditLogs.Add(new AuditLog
                    {
                        KeyPropertyValue = auditLogEntry.KeyPropertyValue,
                        EntityName = auditLogEntry.EntityName,
                        OperationType = auditLogEntry.AuditLogOperationType.ToString(),
                        OldValue = auditLogEntry.OldValue,
                        NewValue = auditLogEntry.NewValue,
                        EnvironmentData = serializedEnvironmentData
                    });
                } 
                
                await _auditLog.AddRangeAsync(auditLogs, cancellationToken);
            }
        }
         
        #endregion
    }
}