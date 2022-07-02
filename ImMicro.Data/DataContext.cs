using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using HelpersToolbox.Extensions;
using ImMicro.Common.Application;
using ImMicro.Data.SeedData;
using ImMicro.Model.Category;
using ImMicro.Model.Product;
using Microsoft.EntityFrameworkCore;
using ImMicro.Model.User;

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

            modelBuilder.Entity<User>().HasData(FakeData.Users);
            modelBuilder.Entity<Category>().HasData(FakeData.Categories);
            modelBuilder.Entity<Product>().HasData(FakeData.Products);
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
                currentUserId = ApplicationContext.Instance.CurrentUser?.Id;
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