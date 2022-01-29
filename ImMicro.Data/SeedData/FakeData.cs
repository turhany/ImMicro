using System;
using System.Collections.Generic;
using ImMicro.Model.Category;
using ImMicro.Model.Product;
using ImMicro.Model.User;

namespace ImMicro.Data.SeedData;

public static class FakeData
{
    public static readonly List<User> Users = new List<User>
    {
        new User
        {
            Id = Guid.Parse("4d7043bf-e857-4d21-9d5e-8926be7e36aa"),
            FirstName = "User",
            LastName = "ImMicro",
            Email = "user@immicro.com",
            Password = BCrypt.Net.BCrypt.HashPassword("123456789.tY"),
            Type = UserType.Root,
            CreatedOn = DateTime.UtcNow,
            RefreshTokenExpireDate = DateTime.UtcNow
        }
    };

    public static readonly List<Category> Categories = new List<Category>
    {
        new Category
        {
            Id = Guid.Parse("471c4da0-35c3-4938-b068-b1ad568118a1"),
            Name = "Computer",
            MinStockQuantity = 10,
            CreatedOn = DateTime.UtcNow,
        },
        new Category
        {
            Id = Guid.Parse("24f5064f-346e-4a1c-a32f-785c0e870d87"),
            Name = "Car",
            MinStockQuantity = 2,
            CreatedOn = DateTime.UtcNow,
        },
        new Category
        {
            Id = Guid.Parse("30ae4b48-5ac1-43c5-b6eb-c08e733b0e46"),
            Name = "Flower",
            MinStockQuantity = 20,
            CreatedOn = DateTime.UtcNow,
        }
    };

    public static readonly List<Product> Products = new List<Product>
    {
        new Product
        {
            Id = Guid.Parse("dcc545de-5afe-4bcd-9883-6b7167af5647"),
            Title = "Macbook Pro 16",
            Description = "Macbook Pro 16 description.",
            StockQuantity = 100,
            IsActive = true,
            CategoryId = Guid.Parse("471c4da0-35c3-4938-b068-b1ad568118a1"),
            CreatedOn = DateTime.UtcNow,
        },
        new Product
        {
            Id = Guid.Parse("d5350cec-78c1-40ac-bc7e-8bc14b09cf64"),
            Title = "Macbook Pro 14",
            Description = "Macbook Pro 14 description.",
            StockQuantity = 9,
            IsActive = false,
            CategoryId = Guid.Parse("471c4da0-35c3-4938-b068-b1ad568118a1"),
            CreatedOn = DateTime.UtcNow,
        },
        new Product
        {
            Id = Guid.Parse("e7ce4ae6-13da-4d8b-834c-7d7a806fc761"),
            Title = "Porche 911",
            Description = "Porche 911 description.",
            StockQuantity = 10,
            IsActive = true,
            CategoryId = Guid.Parse("24f5064f-346e-4a1c-a32f-785c0e870d87"),
            CreatedOn = DateTime.UtcNow,
        },
        new Product
        {
            Id = Guid.Parse("d76632d5-556d-4da6-ae67-2ebfc1b092f3"),
            Title = "Porche 911 Turbo S",
            Description = "Porche 911 Turbo S description.",
            StockQuantity = 1,
            IsActive = false,
            CategoryId = Guid.Parse("24f5064f-346e-4a1c-a32f-785c0e870d87"),
            CreatedOn = DateTime.UtcNow,
        },
        new Product
        {
            Id = Guid.Parse("3884bb77-e0d5-4af9-acd2-a4626a875110"),
            Title = "Rose",
            Description = "Rose flower description.",
            StockQuantity = 10000,
            IsActive = true,
            CategoryId = Guid.Parse("30ae4b48-5ac1-43c5-b6eb-c08e733b0e46"),
            CreatedOn = DateTime.UtcNow,
        },
        new Product
        {
            Id = Guid.Parse("e11d815e-de72-41c3-920e-7f1ca0205aee"),
            Title = "Blue Rose",
            Description = "Blue Rose flower description.",
            StockQuantity = 8,
            IsActive = false,
            CategoryId = Guid.Parse("30ae4b48-5ac1-43c5-b6eb-c08e733b0e46"),
            CreatedOn = DateTime.UtcNow,
        }
    };
}