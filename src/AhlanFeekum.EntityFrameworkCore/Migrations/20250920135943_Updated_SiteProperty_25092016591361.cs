using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AhlanFeekum.Migrations
{
    /// <inheritdoc />
    public partial class Updated_SiteProperty_25092016591361 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Area",
                table: "AppSiteProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "AppSiteProperties",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("c196e05f-9169-43cf-bcf8-0500eaa7e1b7"));

            migrationBuilder.CreateIndex(
                name: "IX_AppSiteProperties_OwnerId",
                table: "AppSiteProperties",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSiteProperties_AppUserProfiles_OwnerId",
                table: "AppSiteProperties",
                column: "OwnerId",
                principalTable: "AppUserProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSiteProperties_AppUserProfiles_OwnerId",
                table: "AppSiteProperties");

            migrationBuilder.DropIndex(
                name: "IX_AppSiteProperties_OwnerId",
                table: "AppSiteProperties");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "AppSiteProperties");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "AppSiteProperties");
        }
    }
}
