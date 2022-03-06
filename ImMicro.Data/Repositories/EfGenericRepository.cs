using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Audity.Extensions;
using Audity.Model;
using ImMicro.Common.Application;
using ImMicro.Common.Data;
using Microsoft.EntityFrameworkCore;
using ImMicro.Common.Data.Abstract;
using ImMicro.Model.AuditLog;
using Newtonsoft.Json;
using Npgsql.Bulk;
using Throw;

// ReSharper disable NotResolvedInText

namespace ImMicro.Data.Repositories
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

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            entity.ThrowIfNull();
            
            await _entities.AddAsync(entity);
            await SaveAuditLogsAsync();
            await _context.SaveEntitiesAsync();
            _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task<List<TEntity>> InsertManyAsync(List<TEntity> entityList)
        {
            entityList.ThrowIfNull();
            entityList.Throw().IfEmpty();
             
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
            entity.ThrowIfNull();
            
            await SaveAuditLogsAsync();
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveEntitiesAsync();
            _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task<List<TEntity>> UpdateManyAsync(List<TEntity> entityList)
        {
            entityList.ThrowIfNull();
            entityList.Throw().IfEmpty();

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
            entity.ThrowIfNull();
            
            if (entity is SoftDeleteEntity softDeleteEntity)
            {
                SetDeleteFields(softDeleteEntity); 
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
            entityList.ThrowIfNull();
            entityList.Throw().IfEmpty();

            if (entityList.First() is SoftDeleteEntity)
            {
                foreach (var entity in entityList)
                {
                    SetDeleteFields(entity as SoftDeleteEntity);
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

        public async Task BulkUpdateAsync(IEnumerable<TEntity> entities)
        {
            var bulkUploader = new NpgsqlBulkUploader(_context);
            await bulkUploader.UpdateAsync(entities);
        }

        public void BulkUpdate(IEnumerable<TEntity> entities)
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