using System;
using AhlanFeekum.Statuses;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AhlanFeekum.Migrations
{
    /// <inheritdoc />
    public partial class Updated_SiteProperty_25092020191244 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "AppSiteProperties",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("418df7ef-4d26-1a23-f03a-3a1c7a6c6a7e"));

            migrationBuilder.CreateIndex(
                name: "IX_AppSiteProperties_StatusId",
                table: "AppSiteProperties",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSiteProperties_AppStatuses_StatusId",
                table: "AppSiteProperties",
                column: "StatusId",
                principalTable: "AppStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSiteProperties_AppStatuses_StatusId",
                table: "AppSiteProperties");

            migrationBuilder.DropIndex(
                name: "IX_AppSiteProperties_StatusId",
                table: "AppSiteProperties");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "AppSiteProperties");
        }
    }
}
