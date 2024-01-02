using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class SkillHub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbsenceType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Has no Type")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbsenceType_Id", x => x.Id);
                    table.UniqueConstraint("AK_AbsenceType_Guid", x => x.TypeGuid);
                });

            migrationBuilder.CreateTable(
                name: "Absence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AbsenceGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    AbsenceTypeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    AbsenceStart = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    AbsenceEnd = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    AbcenceTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Absence_Id", x => x.Id);
                    table.UniqueConstraint("AK_Absence_Guid", x => x.AbsenceGuid);
                    table.ForeignKey(
                        name: "FK_Absence_AbsenceType_AbcenceTypeId",
                        column: x => x.AbcenceTypeId,
                        principalTable: "AbsenceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absence_AbcenceTypeId",
                table: "Absence",
                column: "AbcenceTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Absence");

            migrationBuilder.DropTable(
                name: "AbsenceType");
        }
    }
}
