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
            
            // modelBuilder.Entity<User>().HasData(new List<User>
            // {
            //     new User
            //     {
            //         Id = Guid.Parse("4d7043bf-e857-4d21-9d5e-8926be7e36aa"),
            //         FirstName = "User",
            //         LastName = "ImMicro",
            //         Email = "user@immicro.com",
            //         Password = BCrypt.Net.BCrypt.HashPassword("123456789.tY"),
            //         Type = UserType.Root,
            //         CreatedOn = DateTime.UtcNow,
            //         IsActive = true,
            //         RefreshTokenExpireDate = DateTime.UtcNow
            //     }
            // });
            
            // modelBuilder.Entity<Category>().HasData(new List<Category>
            // {
            //     new Category
            //     {
            //         Id = Guid.Parse("471c4da0-35c3-4938-b068-b1ad568118a1"),
            //         Name = "Computer",
            //         MinStockQuantity = 10,
            //         CreatedOn = DateTime.UtcNow,
            //     },
            //     new Category
            //     {
            //         Id = Guid.Parse("24f5064f-346e-4a1c-a32f-785c0e870d87"),
            //         Name = "Car",
            //         MinStockQuantity = 2,
            //         CreatedOn = DateTime.UtcNow,
            //     },
            //     new Category
            //     {
            //         Id = Guid.Parse("30ae4b48-5ac1-43c5-b6eb-c08e733b0e46"),
            //         Name = "Flower",
            //         MinStockQuantity = 20,
            //         CreatedOn = DateTime.UtcNow,
            //     }
            // });
            
            // modelBuilder.Entity<Product>().HasData(new List<Product>
            // {
            //     new Product
            //     {
            //         Id = Guid.Parse("dcc545de-5afe-4bcd-9883-6b7167af5647"),
            //         Title = "Macbook Pro 16",
            //         Description = "Macbook Pro 16 description.",
            //         StockQuantity = 100,
            //         CategoryId = Guid.Parse("471c4da0-35c3-4938-b068-b1ad568118a1"),
            //         CreatedOn = DateTime.UtcNow,
            //     },
            //     new Product
            //     {
            //         Id = Guid.Parse("d5350cec-78c1-40ac-bc7e-8bc14b09cf64"),
            //         Title = "Macbook Pro 14",
            //         Description = "Macbook Pro 14 description.",
            //         StockQuantity = 9,
            //         IsActive = false,
            //         CategoryId = Guid.Parse("471c4da0-35c3-4938-b068-b1ad568118a1"),
            //         CreatedOn = DateTime.UtcNow,
            //     },
            //     new Product
            //     {
            //         Id = Guid.Parse("e7ce4ae6-13da-4d8b-834c-7d7a806fc761"),
            //         Title = "Porche 911",
            //         Description = "Porche 911 description.",
            //         StockQuantity = 10,
            //         CategoryId = Guid.Parse("24f5064f-346e-4a1c-a32f-785c0e870d87"),
            //         CreatedOn = DateTime.UtcNow,
            //     },
            //     new Product
            //     {
            //         Id = Guid.Parse("d76632d5-556d-4da6-ae67-2ebfc1b092f3"),
            //         Title = "Porche 911 Turbo S",
            //         Description = "Porche 911 Turbo S description.",
            //         StockQuantity = 1,
            //         IsActive = false,
            //         CategoryId = Guid.Parse("24f5064f-346e-4a1c-a32f-785c0e870d87"),
            //         CreatedOn = DateTime.UtcNow,
            //     },
            //     new Product
            //     {
            //         Id = Guid.Parse("3884bb77-e0d5-4af9-acd2-a4626a875110"),
            //         Title = "Rose",
            //         Description = "Rose flower description.",
            //         StockQuantity = 10000,
            //         CategoryId = Guid.Parse("30ae4b48-5ac1-43c5-b6eb-c08e733b0e46"),
            //         CreatedOn = DateTime.UtcNow,
            //     },
            //     new Product
            //     {
            //         Id = Guid.Parse("e11d815e-de72-41c3-920e-7f1ca0205aee"),
            //         Title = "Blue Rose",
            //         Description = "Blue Rose flower description.",
            //         StockQuantity = 8,
            //         IsActive = false,
            //         CategoryId = Guid.Parse("30ae4b48-5ac1-43c5-b6eb-c08e733b0e46"),
            //         CreatedOn = DateTime.UtcNow,
            //     }
            // });
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
                currentUserId = ApplicationContext.Instance.CurrentUser.Id;
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