using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AhlanFeekum.Migrations
{
    /// <inheritdoc />
    public partial class Updated_AhlanfeekumTerm_25092915200653 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TermsIconExtension",
                table: "AppAhlanfeekumTerms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WhoAreWeIconExtension",
                table: "AppAhlanfeekumTerms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TermsIconExtension",
                table: "AppAhlanfeekumTerms");

            migrationBuilder.DropColumn(
                name: "WhoAreWeIconExtension",
                table: "AppAhlanfeekumTerms");
        }
    }
}
