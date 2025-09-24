using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AhlanFeekum.Migrations
{
    /// <inheritdoc />
    public partial class Added_Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppStatuses", x => x.Id);
                });

            migrationBuilder.InsertData(
       table: "AppStatuses",
       columns: new[] { "Id", "ExtraProperties", "ConcurrencyStamp", "CreationTime", "IsDeleted", "Name", "Order", "IsActive" },
       values: new object[,]
       {
            { Guid.Parse("418df7ef-4d26-1a23-f03a-3a1c7a6c6a7e"), "{}", Guid.NewGuid().ToString(), DateTime.UtcNow, false, "Pending", 1, true },
            { Guid.NewGuid(), "{}", Guid.NewGuid().ToString(), DateTime.UtcNow, false, "Approved", 2, true },
            { Guid.NewGuid(), "{}", Guid.NewGuid().ToString(), DateTime.UtcNow, false, "Decline", 3, true }
       });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppStatuses");
        }
    }
}
