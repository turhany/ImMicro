using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Audity.Extensions;
using Audity.Model;
using HelpersToolbox.Extensions;
using ImMicro.Common.Application;
using Microsoft.EntityFrameworkCore;
using ImMicro.Common.Data.Abstract;
using ImMicro.Model.AuditLog;
using Newtonsoft.Json;
using Npgsql.Bulk;

namespace ImMicro.Data.Repositories
{
  public class EfGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DataContext _context;
        private readonly DbSet<TEntity> _entities;
        private readonly DbSet<AuditLog> _auditLog;

        private const string IsDeletedFieldName = "IsDeleted";
        private const string DeletedOnFieldName = "DeletedOn";
        private const string DeletedByFieldName = "DeletedBy";

        public EfGenericRepository(DataContext dbContext)
        {
            _context = dbContext;
            _entities = _context.Set<TEntity>();
            _auditLog = _context.Set<AuditLog>();
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            ThrowIfNull(entity);
            
            await _entities.AddAsync(entity);
            await SaveAuditLogsAsync();
            await _context.SaveEntitiesAsync();
            _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task<List<TEntity>> InsertManyAsync(List<TEntity> entityList)
        {
            ThrowIfAnyOneNull(entityList);

            await _entities.AddRangeAsync(entityList);
            await SaveAuditLogsAsync();
            await _context.SaveEntitiesAsync();

            foreach (var entity in entityList)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entityList;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            ThrowIfNull(entity);

            await SaveAuditLogsAsync();
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveEntitiesAsync();
            _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task<List<TEntity>> UpdateManyAsync(List<TEntity> entityList)
        {
            ThrowIfAnyOneNull(entityList);

            await SaveAuditLogsAsync();
            _entities.UpdateRange(entityList);
            await _context.SaveEntitiesAsync();
            foreach (var entity in entityList)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entityList;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            ThrowIfNull(entity);

            
            if (entity.HasProperty(IsDeletedFieldName))
            {
                SetDeleteFields(entity); 
                await UpdateAsync(entity);
            }
            else
            {
                _entities.Remove(entity);
                await SaveAuditLogsAsync();
                await _context.SaveEntitiesAsync();
            }
        }

        public async Task DeleteManyAsync(List<TEntity> entityList)
        {
            ThrowIfAnyOneNull(entityList);

            if (entityList.First().HasProperty(IsDeletedFieldName))
            {
                foreach (var entity in entityList)
                {
                    SetDeleteFields(entity);
                }

                await UpdateManyAsync(entityList);
            }
            else
            {
                _entities.RemoveRange(entityList);
                await SaveAuditLogsAsync();
                await _context.SaveEntitiesAsync();
            }
        }

        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate) 
            => await _entities.FirstOrDefaultAsync(predicate);

        public async Task<TEntity> FindOneWithAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate) 
            => await _entities.AsNoTracking().FirstOrDefaultAsync(predicate);

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) 
            => _entities.Where(predicate);

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate) 
            => await _entities.AnyAsync(predicate);

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate) 
            => await _entities.CountAsync(predicate);

        public async Task BulkInsertAsync(IEnumerable<TEntity> entities)
        {
            var bulkUploader = new NpgsqlBulkUploader(_context);
            await bulkUploader.InsertAsync(entities);
        }
        
        public void BulkInsert(IEnumerable<TEntity> entities)
        {
            var bulkUploader = new NpgsqlBulkUploader(_context);
            bulkUploader.Insert(entities);
        }
        
        #region Private Methods

        private void ThrowIfNull(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
        }

        private void ThrowIfAnyOneNull(List<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            if (entities.Any(p => p == null)) throw new ArgumentNullException("entities has null item(s).");
        }

        private void SetDeleteFields(TEntity entity)
        {
            entity.SetPropertyValue(IsDeletedFieldName, true);
            entity.SetPropertyValue(DeletedOnFieldName, DateTime.UtcNow);
            entity.SetPropertyValue(DeletedByFieldName, ApplicationContext.Instance.CurrentUser);
        }

        private async Task SaveAuditLogsAsync()
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
                
                await _auditLog.AddRangeAsync(auditLogs);
            }
        }
        
        #endregion
    }
}