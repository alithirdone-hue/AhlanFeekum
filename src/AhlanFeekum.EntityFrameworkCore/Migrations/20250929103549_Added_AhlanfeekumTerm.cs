using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AhlanFeekum.Migrations
{
    /// <inheritdoc />
    public partial class Added_AhlanfeekumTerm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppAhlanfeekumTerms",
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
                    TermsTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermsAnnotation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermsDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermsIconId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WhoAreWeTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhoAreWeAnnotation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhoAreWeDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhoAreWeIconId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAhlanfeekumTerms", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppAhlanfeekumTerms");
        }
    }
}
