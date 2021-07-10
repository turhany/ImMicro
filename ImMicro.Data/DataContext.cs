using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ImMicro.Common.Extensions;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable HeuristicUnreachableCode
// ReSharper disable InvalidXmlDocComment

namespace ImMicro.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveEntitiesAsync()
        {
            SetAuditProperties();
            return await SaveChangesAsync();
        }

        private const string CreatedOnFieldName = "CreatedOn";
        private const string UpdatedOnFieldName = "UpdatedOn";
        private const string CreatedByFieldName = "CreatedBy";
        private const string UpdatedByFieldName = "UpdatedBy";

        private void SetAuditProperties()
        {
            Guid? currentUserId = null;
            try
            {
                ///INFO: get user some where implement it
            }
            catch
            {
                //User not initialized ignore
            }

            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity.HasProperty(CreatedOnFieldName))
                        entry.Entity.SetPropertyValue(CreatedOnFieldName, DateTime.UtcNow);

                    if (entry.Entity.HasProperty(CreatedByFieldName) && currentUserId.HasValue)
                        entry.Entity.SetPropertyValue(CreatedByFieldName, currentUserId.Value);
                }
                else if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    if (entry.Entity.HasProperty(UpdatedOnFieldName))
                        entry.Entity.SetPropertyValue(UpdatedOnFieldName, DateTime.UtcNow);

                    if (entry.Entity.HasProperty(UpdatedByFieldName) && currentUserId.HasValue)
                        entry.Entity.SetPropertyValue(UpdatedByFieldName, currentUserId.Value);
                }
            }
        } 
    }
}