using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImMicro.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KeyPropertyValue = table.Column<string>(type: "text", nullable: true),
                    EntityName = table.Column<string>(type: "text", nullable: true),
                    OperationType = table.Column<string>(type: "text", nullable: true),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    EnvironmentData = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    MinStockQuantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Request = table.Column<string>(type: "text", nullable: true),
                    Response = table.Column<string>(type: "text", nullable: true),
                    StatusCode = table.Column<string>(type: "text", nullable: true),
                    RequestPath = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "IsDeleted", "MinStockQuantity", "Name", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { new Guid("24f5064f-346e-4a1c-a32f-785c0e870d87"), null, new DateTime(2022, 1, 29, 7, 59, 0, 747, DateTimeKind.Utc).AddTicks(9638), null, null, false, 2, "Car", null, null },
                    { new Guid("30ae4b48-5ac1-43c5-b6eb-c08e733b0e46"), null, new DateTime(2022, 1, 29, 7, 59, 0, 747, DateTimeKind.Utc).AddTicks(9640), null, null, false, 20, "Flower", null, null },
                    { new Guid("471c4da0-35c3-4938-b068-b1ad568118a1"), null, new DateTime(2022, 1, 29, 7, 59, 0, 747, DateTimeKind.Utc).AddTicks(9627), null, null, false, 10, "Computer", null, null }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "Email", "FirstName", "IsActive", "IsDeleted", "LastName", "Password", "RefreshToken", "RefreshTokenExpireDate", "Type", "UpdatedBy", "UpdatedOn" },
                values: new object[] { new Guid("4d7043bf-e857-4d21-9d5e-8926be7e36aa"), null, new DateTime(2022, 1, 29, 7, 59, 0, 747, DateTimeKind.Utc).AddTicks(8369), null, null, "user@immicro.com", "User", true, false, "ImMicro", "$2a$11$1m6jXZOiglvCh4s5LkIv/uevaYGQPuBvTrzIZVL5qvrkPUvHzPZKm", null, new DateTime(2022, 1, 29, 7, 59, 0, 747, DateTimeKind.Utc).AddTicks(8758), 1, null, null });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "CategoryId", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "Description", "IsActive", "IsDeleted", "StockQuantity", "Title", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { new Guid("3884bb77-e0d5-4af9-acd2-a4626a875110"), new Guid("30ae4b48-5ac1-43c5-b6eb-c08e733b0e46"), null, new DateTime(2022, 1, 29, 7, 59, 0, 748, DateTimeKind.Utc).AddTicks(549), null, null, "Rose flower description.", true, false, 10000, "Rose", null, null },
                    { new Guid("d5350cec-78c1-40ac-bc7e-8bc14b09cf64"), new Guid("471c4da0-35c3-4938-b068-b1ad568118a1"), null, new DateTime(2022, 1, 29, 7, 59, 0, 748, DateTimeKind.Utc).AddTicks(539), null, null, "Macbook Pro 14 description.", false, false, 9, "Macbook Pro 14", null, null },
                    { new Guid("d76632d5-556d-4da6-ae67-2ebfc1b092f3"), new Guid("24f5064f-346e-4a1c-a32f-785c0e870d87"), null, new DateTime(2022, 1, 29, 7, 59, 0, 748, DateTimeKind.Utc).AddTicks(547), null, null, "Porche 911 Turbo S description.", false, false, 1, "Porche 911 Turbo S", null, null },
                    { new Guid("dcc545de-5afe-4bcd-9883-6b7167af5647"), new Guid("471c4da0-35c3-4938-b068-b1ad568118a1"), null, new DateTime(2022, 1, 29, 7, 59, 0, 748, DateTimeKind.Utc).AddTicks(525), null, null, "Macbook Pro 16 description.", true, false, 100, "Macbook Pro 16", null, null },
                    { new Guid("e11d815e-de72-41c3-920e-7f1ca0205aee"), new Guid("30ae4b48-5ac1-43c5-b6eb-c08e733b0e46"), null, new DateTime(2022, 1, 29, 7, 59, 0, 748, DateTimeKind.Utc).AddTicks(557), null, null, "Blue Rose flower description.", false, false, 8, "Blue Rose", null, null },
                    { new Guid("e7ce4ae6-13da-4d8b-834c-7d7a806fc761"), new Guid("24f5064f-346e-4a1c-a32f-785c0e870d87"), null, new DateTime(2022, 1, 29, 7, 59, 0, 748, DateTimeKind.Utc).AddTicks(545), null, null, "Porche 911 description.", true, false, 10, "Porche 911", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "RequestLog");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
