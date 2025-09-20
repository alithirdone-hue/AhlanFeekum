using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AhlanFeekum.Migrations
{
    /// <inheritdoc />
    public partial class Updated_OnlyForYouSection_25091807031868 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstPhotoExtension",
                table: "AppOnlyForYouSections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SecondPhotoExtension",
                table: "AppOnlyForYouSections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ThirdPhotoExtension",
                table: "AppOnlyForYouSections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstPhotoExtension",
                table: "AppOnlyForYouSections");

            migrationBuilder.DropColumn(
                name: "SecondPhotoExtension",
                table: "AppOnlyForYouSections");

            migrationBuilder.DropColumn(
                name: "ThirdPhotoExtension",
                table: "AppOnlyForYouSections");
        }
    }
}
