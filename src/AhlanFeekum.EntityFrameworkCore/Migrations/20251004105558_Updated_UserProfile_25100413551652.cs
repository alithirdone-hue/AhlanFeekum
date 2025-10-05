using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AhlanFeekum.Migrations
{
    /// <inheritdoc />
    public partial class Updated_UserProfile_25100413551652 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FcmToken",
                table: "AppUserProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FcmToken",
                table: "AppUserProfiles");
        }
    }
}
