using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class SkillHub3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Absence_AbsenceTypeGuid",
                table: "Absence",
                column: "AbsenceTypeGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_AbsenceType_AbsenceTypeGuid",
                table: "Absence",
                column: "AbsenceTypeGuid",
                principalTable: "AbsenceType",
                principalColumn: "TypeGuid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absence_AbsenceType_AbsenceTypeGuid",
                table: "Absence");

            migrationBuilder.DropIndex(
                name: "IX_Absence_AbsenceTypeGuid",
                table: "Absence");
        }
    }
}
