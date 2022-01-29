﻿// <auto-generated />
using System;
using ImMicro.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ImMicro.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ImMicro.Model.AuditLog.AuditLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EntityName")
                        .HasColumnType("text");

                    b.Property<string>("EnvironmentData")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("KeyPropertyValue")
                        .HasColumnType("text");

                    b.Property<string>("NewValue")
                        .HasColumnType("text");

                    b.Property<string>("OldValue")
                        .HasColumnType("text");

                    b.Property<string>("OperationType")
                        .HasColumnType("text");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("AuditLog", (string)null);
                });

            modelBuilder.Entity("ImMicro.Model.Category.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("MinStockQuantity")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Category", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("471c4da0-35c3-4938-b068-b1ad568118a1"),
                            CreatedOn = new DateTime(2022, 1, 29, 22, 4, 17, 550, DateTimeKind.Utc).AddTicks(5544),
                            IsDeleted = false,
                            MinStockQuantity = 10,
                            Name = "Computer"
                        },
                        new
                        {
                            Id = new Guid("24f5064f-346e-4a1c-a32f-785c0e870d87"),
                            CreatedOn = new DateTime(2022, 1, 29, 22, 4, 17, 550, DateTimeKind.Utc).AddTicks(5560),
                            IsDeleted = false,
                            MinStockQuantity = 2,
                            Name = "Car"
                        },
                        new
                        {
                            Id = new Guid("30ae4b48-5ac1-43c5-b6eb-c08e733b0e46"),
                            CreatedOn = new DateTime(2022, 1, 29, 22, 4, 17, 550, DateTimeKind.Utc).AddTicks(5562),
                            IsDeleted = false,
                            MinStockQuantity = 20,
                            Name = "Flower"
                        });
                });

            modelBuilder.Entity("ImMicro.Model.Product.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("dcc545de-5afe-4bcd-9883-6b7167af5647"),
                            CategoryId = new Guid("471c4da0-35c3-4938-b068-b1ad568118a1"),
                            CreatedOn = new DateTime(2022, 1, 29, 22, 4, 17, 550, DateTimeKind.Utc).AddTicks(6411),
                            Description = "Macbook Pro 16 description.",
                            IsActive = true,
                            IsDeleted = false,
                            StockQuantity = 100,
                            Title = "Macbook Pro 16"
                        },
                        new
                        {
                            Id = new Guid("d5350cec-78c1-40ac-bc7e-8bc14b09cf64"),
                            CategoryId = new Guid("471c4da0-35c3-4938-b068-b1ad568118a1"),
                            CreatedOn = new DateTime(2022, 1, 29, 22, 4, 17, 550, DateTimeKind.Utc).AddTicks(6426),
                            Description = "Macbook Pro 14 description.",
                            IsActive = false,
                            IsDeleted = false,
                            StockQuantity = 9,
                            Title = "Macbook Pro 14"
                        },
                        new
                        {
                            Id = new Guid("e7ce4ae6-13da-4d8b-834c-7d7a806fc761"),
                            CategoryId = new Guid("24f5064f-346e-4a1c-a32f-785c0e870d87"),
                            CreatedOn = new DateTime(2022, 1, 29, 22, 4, 17, 550, DateTimeKind.Utc).AddTicks(6428),
                            Description = "Porche 911 description.",
                            IsActive = true,
                            IsDeleted = false,
                            StockQuantity = 10,
                            Title = "Porche 911"
                        },
                        new
                        {
                            Id = new Guid("d76632d5-556d-4da6-ae67-2ebfc1b092f3"),
                            CategoryId = new Guid("24f5064f-346e-4a1c-a32f-785c0e870d87"),
                            CreatedOn = new DateTime(2022, 1, 29, 22, 4, 17, 550, DateTimeKind.Utc).AddTicks(6431),
                            Description = "Porche 911 Turbo S description.",
                            IsActive = false,
                            IsDeleted = false,
                            StockQuantity = 1,
                            Title = "Porche 911 Turbo S"
                        },
                        new
                        {
                            Id = new Guid("3884bb77-e0d5-4af9-acd2-a4626a875110"),
                            CategoryId = new Guid("30ae4b48-5ac1-43c5-b6eb-c08e733b0e46"),
                            CreatedOn = new DateTime(2022, 1, 29, 22, 4, 17, 550, DateTimeKind.Utc).AddTicks(6436),
                            Description = "Rose flower description.",
                            IsActive = true,
                            IsDeleted = false,
                            StockQuantity = 10000,
                            Title = "Rose"
                        },
                        new
                        {
                            Id = new Guid("e11d815e-de72-41c3-920e-7f1ca0205aee"),
                            CategoryId = new Guid("30ae4b48-5ac1-43c5-b6eb-c08e733b0e46"),
                            CreatedOn = new DateTime(2022, 1, 29, 22, 4, 17, 550, DateTimeKind.Utc).AddTicks(6447),
                            Description = "Blue Rose flower description.",
                            IsActive = false,
                            IsDeleted = false,
                            StockQuantity = 8,
                            Title = "Blue Rose"
                        });
                });

            modelBuilder.Entity("ImMicro.Model.RequestLog.RequestLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Request")
                        .HasColumnType("text");

                    b.Property<string>("RequestPath")
                        .HasColumnType("text");

                    b.Property<string>("Response")
                        .HasColumnType("text");

                    b.Property<string>("StatusCode")
                        .HasColumnType("text");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("RequestLog", (string)null);
                });

            modelBuilder.Entity("ImMicro.Model.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<DateTime>("RefreshTokenExpireDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("4d7043bf-e857-4d21-9d5e-8926be7e36aa"),
                            CreatedOn = new DateTime(2022, 1, 29, 22, 4, 17, 550, DateTimeKind.Utc).AddTicks(4447),
                            Email = "user@immicro.com",
                            FirstName = "User",
                            IsDeleted = false,
                            LastName = "ImMicro",
                            Password = "$2a$11$8FdieqBtwfeRxM7XR.t39e6DJotDXEPOnUQ52QoiGJRq4tFW7XJTq",
                            RefreshTokenExpireDate = new DateTime(2022, 1, 29, 22, 4, 17, 550, DateTimeKind.Utc).AddTicks(4686),
                            Type = 1
                        });
                });

            modelBuilder.Entity("ImMicro.Model.Product.Product", b =>
                {
                    b.HasOne("ImMicro.Model.Category.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}
