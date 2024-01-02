using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class SkillHub2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absences_AbsenceTypes_AbcenceTypeId",
                table: "Absences");

            migrationBuilder.DropIndex(
                name: "IX_Absences_AbcenceTypeId",
                table: "Absences");

            migrationBuilder.DropColumn(
                name: "AbcenceTypeId",
                table: "Absences");

            migrationBuilder.RenameTable(
                name: "AbsenceTypes",
                newName: "AbsenceType");

            migrationBuilder.RenameTable(
                name: "Absences",
                newName: "Absence");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "AbsenceType",
                newName: "AbsenceTypes");

            migrationBuilder.RenameTable(
                name: "Absence",
                newName: "Absences");

            migrationBuilder.AddColumn<int>(
                name: "AbcenceTypeId",
                table: "Absences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Absences_AbcenceTypeId",
                table: "Absences",
                column: "AbcenceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absences_AbsenceTypes_AbcenceTypeId",
                table: "Absences",
                column: "AbcenceTypeId",
                principalTable: "AbsenceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
