using HelpersToolbox.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ImMicro.Common.Application;
using ImMicro.Data.MongoDB.Options;
using System.Linq.Expressions;
using Throw;
using ImMicro.Data.BaseRepositories;
using ImMicro.Data.BaseModels;
using ImMicro.Resources.Model;

namespace ImMicro.Data.MongoDB.Repositories
{
    public class MongoDBGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
    {
        protected readonly IMongoCollection<TEntity> Collection;

        public MongoDBGenericRepository(IOptions<MongoDBOption> mongoDBOption)
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
            var client = new MongoClient(mongoDBOption.Value.ConnectionString);
            var database = client.GetDatabase(mongoDBOption.Value.Database);
            Collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.ThrowIfNull();

            SetAuditFields(entity, OperationFlow.Insert);

            var options = new InsertOneOptions { BypassDocumentValidation = false };
            await Collection.InsertOneAsync(entity, options, cancellationToken);
            return entity;
        }
        public async Task<List<TEntity>> InsertManyAsync(List<TEntity> entityList, CancellationToken cancellationToken)
        {
            entityList.ThrowIfNull();
            entityList.Throw().IfEmpty();

            SetAuditFields(entityList, OperationFlow.Insert);

            await Collection.InsertManyAsync(entityList, null, cancellationToken);
            return entityList;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.ThrowIfNull();

            SetAuditFields(entity, OperationFlow.Update);

            await Collection.ReplaceOneAsync(model => model.Id == entity.Id, entity, new ReplaceOptions(), cancellationToken);

            return await FindOneAsync(p => p.Id == entity.Id, cancellationToken);
        }
        public async Task<List<TEntity>> UpdateManyAsync(List<TEntity> entityList, CancellationToken cancellationToken)
        {
            entityList.ThrowIfNull();
            entityList.Throw().IfEmpty();

            for (int i = 0; i < entityList.Count; i++)
            {
                entityList[i] = await UpdateAsync(entityList[i], cancellationToken);
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
                await Collection.DeleteOneAsync(model => model.Id == entity.Id, cancellationToken);
            }
        }
        public async Task DeleteManyAsync(List<TEntity> entityList, CancellationToken cancellationToken)
        {
            entityList.ThrowIfNull();
            entityList.Throw().IfEmpty();

            foreach (var entity in entityList)
            {
                await DeleteAsync(entity, cancellationToken);
            }
        }

        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await Collection.Find(predicate).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<TEntity> FindOneWithAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await FindOneAsync(predicate, cancellationToken);
        }
        public IQueryable<TEntity> AsQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return Collection.AsQueryable().Where(predicate);
        }
        public async Task<List<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await Collection.Find(predicate).ToListAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await Collection.Find(predicate).AnyAsync(cancellationToken);
        }
        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await Collection.Find(predicate).CountDocumentsAsync(cancellationToken);
        }

        public async Task BulkInsertAsync(List<TEntity> entityList, CancellationToken cancellationToken)
        {
            entityList.ThrowIfNull();
            entityList.Throw().IfEmpty();

            var insertList = new List<WriteModel<TEntity>>();

            foreach (var entity in entityList)
            {
                insertList.Add(new InsertOneModel<TEntity>(entity));
            }

            var options = new BulkWriteOptions { IsOrdered = false, BypassDocumentValidation = false };
            await Collection.BulkWriteAsync(insertList, options, cancellationToken);
        }
        public async Task BulkUpdateAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            await UpdateManyAsync(entities, cancellationToken);
        }

        #region Private Methods

        private void SetAuditFields(TEntity entity, OperationFlow operationFlow)
        {
            Guid? currentUserId = ApplicationContext.Instance?.CurrentUser?.Id;

            switch (operationFlow)
            {
                case MongoDBGenericRepository<TEntity>.OperationFlow.Insert:
                    if (entity.HasProperty(nameof(Entity.CreatedOn)))
                        entity.SetPropertyValue(nameof(Entity.CreatedOn), DateTime.UtcNow);

                    if (entity.HasProperty(nameof(Entity.CreatedBy)) && currentUserId.HasValue)
                        entity.SetPropertyValue(nameof(Entity.CreatedBy), currentUserId.Value);
                    break;
                case MongoDBGenericRepository<TEntity>.OperationFlow.Update:
                    if (entity.HasProperty(nameof(Entity.UpdatedOn)))
                        entity.SetPropertyValue(nameof(Entity.UpdatedOn), DateTime.UtcNow);

                    if (entity.HasProperty(nameof(Entity.UpdatedBy)) && currentUserId.HasValue)
                        entity.SetPropertyValue(nameof(Entity.UpdatedBy), currentUserId.Value);
                    break;
                default:
                    break;
            }
        }

        private void SetAuditFields(List<TEntity> entityList, OperationFlow operationFlow)
        {
            foreach (var entity in entityList)
            {
                SetAuditFields(entity, operationFlow);
            }
        }

        private void SetDeleteFields(SoftDeleteEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = ApplicationContext.Instance.CurrentUser.Id;
        }

        private enum OperationFlow
        {
            Insert,
            Update
        }

        #endregion
    }
}
