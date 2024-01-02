using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class PeopleManagementMigration_Updatecolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Collaborators_CCNumber",
                table: "Collaborators");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Collaborators_SSNumber",
                table: "Collaborators");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Collaborators_TaxNumber",
                table: "Collaborators");

            migrationBuilder.RenameTable(
                name: "Collaborators",
                newName: "Person");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CCVal",
                table: "Person",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "NoCCExpireDate");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Person_CCNumber",
                table: "Person",
                column: "CCNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Person_SSNumber",
                table: "Person",
                column: "SSNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Person_TaxNumber",
                table: "Person",
                column: "TaxNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Person_CCNumber",
                table: "Person");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Person_SSNumber",
                table: "Person");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Person_TaxNumber",
                table: "Person");

            migrationBuilder.RenameTable(
                name: "Person",
                newName: "Collaborators");

            migrationBuilder.AlterColumn<string>(
                name: "CCVal",
                table: "Collaborators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "NoCCExpireDate",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Collaborators_CCNumber",
                table: "Collaborators",
                column: "CCNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Collaborators_SSNumber",
                table: "Collaborators",
                column: "SSNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Collaborators_TaxNumber",
                table: "Collaborators",
                column: "TaxNumber");
        }
    }
}
