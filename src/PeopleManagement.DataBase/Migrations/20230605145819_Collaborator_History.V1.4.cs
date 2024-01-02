using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class Collaborator_HistoryV14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PersonHistory_CCNumber",
                table: "PersonHistory");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PersonHistory_SSNumber",
                table: "PersonHistory");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PersonHistory_TaxNumber",
                table: "PersonHistory");

            migrationBuilder.DropUniqueConstraint(
                name: "Collaborator_CollaboratorGUID",
                table: "PersonHistory");

            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "SSNumber",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CCNumber",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "PersonHistory",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SSNumber",
                table: "PersonHistory",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CCNumber",
                table: "PersonHistory",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PersonHistory_CCNumber",
                table: "PersonHistory",
                column: "CCNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PersonHistory_SSNumber",
                table: "PersonHistory",
                column: "SSNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PersonHistory_TaxNumber",
                table: "PersonHistory",
                column: "TaxNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "Collaborator_CollaboratorGUID",
                table: "PersonHistory",
                column: "PeopleGUID");
        }
    }
}
