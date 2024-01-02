using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class SkillHub6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AbsenceType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "ApprovalStatus",
                table: "Absence",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AbsenceTypeHistoryId",
                table: "Absence",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Absence",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AbsenceHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AbsenceGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    AbsenceTypeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    AbsenceStart = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    AbsenceEnd = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    Schedule = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Full Day"),
                    ActionText = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "None"),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 5, 30, 10, 34, 40, 224, DateTimeKind.Local).AddTicks(669)),
                    AbsenceTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbsenceHistory_Id", x => x.Id);
                    table.UniqueConstraint("AK_AbsenceHistory_Guid", x => x.AbsenceGuid);
                    table.ForeignKey(
                        name: "FK_AbsenceHistory_AbsenceType_AbsenceTypeId",
                        column: x => x.AbsenceTypeId,
                        principalTable: "AbsenceType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AbsenceTypeHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Has no Type"),
                    ActionText = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "None"),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 5, 30, 10, 34, 40, 224, DateTimeKind.Local).AddTicks(1778))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbsenceTypeHistory_Id", x => x.Id);
                    table.UniqueConstraint("AK_AbsenceTypeHistory_Guid", x => x.TypeGuid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absence_AbsenceTypeHistoryId",
                table: "Absence",
                column: "AbsenceTypeHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AbsenceHistory_AbsenceTypeId",
                table: "AbsenceHistory",
                column: "AbsenceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_AbsenceTypeHistory_AbsenceTypeHistoryId",
                table: "Absence",
                column: "AbsenceTypeHistoryId",
                principalTable: "AbsenceTypeHistory",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absence_AbsenceTypeHistory_AbsenceTypeHistoryId",
                table: "Absence");

            migrationBuilder.DropTable(
                name: "AbsenceHistory");

            migrationBuilder.DropTable(
                name: "AbsenceTypeHistory");

            migrationBuilder.DropIndex(
                name: "IX_Absence_AbsenceTypeHistoryId",
                table: "Absence");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AbsenceType");

            migrationBuilder.DropColumn(
                name: "AbsenceTypeHistoryId",
                table: "Absence");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Absence");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovalStatus",
                table: "Absence",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);
        }
    }
}
