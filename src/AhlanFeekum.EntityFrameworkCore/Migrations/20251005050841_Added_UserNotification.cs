using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AhlanFeekum.Migrations
{
    /// <inheritdoc />
    public partial class Added_UserNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUserNotifications",
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
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserNotificationSiteProperty",
                columns: table => new
                {
                    UserNotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SitePropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserNotificationSiteProperty", x => new { x.UserNotificationId, x.SitePropertyId });
                    table.ForeignKey(
                        name: "FK_AppUserNotificationSiteProperty_AppSiteProperties_SitePropertyId",
                        column: x => x.SitePropertyId,
                        principalTable: "AppSiteProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserNotificationSiteProperty_AppUserNotifications_UserNotificationId",
                        column: x => x.UserNotificationId,
                        principalTable: "AppUserNotifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserNotificationUserProfile",
                columns: table => new
                {
                    UserNotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserNotificationUserProfile", x => new { x.UserNotificationId, x.UserProfileId });
                    table.ForeignKey(
                        name: "FK_AppUserNotificationUserProfile_AppUserNotifications_UserNotificationId",
                        column: x => x.UserNotificationId,
                        principalTable: "AppUserNotifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserNotificationUserProfile_AppUserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "AppUserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserNotificationSiteProperty_SitePropertyId",
                table: "AppUserNotificationSiteProperty",
                column: "SitePropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserNotificationSiteProperty_UserNotificationId_SitePropertyId",
                table: "AppUserNotificationSiteProperty",
                columns: new[] { "UserNotificationId", "SitePropertyId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserNotificationUserProfile_UserNotificationId_UserProfileId",
                table: "AppUserNotificationUserProfile",
                columns: new[] { "UserNotificationId", "UserProfileId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserNotificationUserProfile_UserProfileId",
                table: "AppUserNotificationUserProfile",
                column: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserNotificationSiteProperty");

            migrationBuilder.DropTable(
                name: "AppUserNotificationUserProfile");

            migrationBuilder.DropTable(
                name: "AppUserNotifications");
        }
    }
}
