using System.Reflection; 
using HelpersToolbox.Extensions;
using ImMicro.Common.Application; 
using ImMicro.Model.Category;
using ImMicro.Model.Product;
using Microsoft.EntityFrameworkCore;
using ImMicro.Model.User; 
using ImMicro.Data.EntityFramework.SeedData;
 
namespace ImMicro.Data.EntityFramework
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

        public async Task<int> SaveEntitiesAsync(CancellationToken cancellationToken)
        {
            SetAuditProperties();
            return await SaveChangesAsync(cancellationToken);
        }

        private const string CreatedOnFieldName = "CreatedOn";
        private const string UpdatedOnFieldName = "UpdatedOn";
        private const string CreatedByFieldName = "CreatedBy";
        private const string UpdatedByFieldName = "UpdatedBy";

        private void SetAuditProperties()
        {
            Guid? currentUserId = ApplicationContext.Instance?.CurrentUser?.Id;

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