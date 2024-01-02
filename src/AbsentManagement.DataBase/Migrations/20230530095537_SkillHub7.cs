using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class SkillHub7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AbsenceTypeHistory_Guid",
                table: "AbsenceTypeHistory");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AbsenceHistory_Guid",
                table: "AbsenceHistory");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceTypeHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 30, 10, 55, 37, 693, DateTimeKind.Local).AddTicks(743),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 30, 10, 34, 40, 224, DateTimeKind.Local).AddTicks(1778));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 30, 10, 55, 37, 692, DateTimeKind.Local).AddTicks(9908),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 30, 10, 34, 40, 224, DateTimeKind.Local).AddTicks(669));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceTypeHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 30, 10, 34, 40, 224, DateTimeKind.Local).AddTicks(1778),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 30, 10, 55, 37, 693, DateTimeKind.Local).AddTicks(743));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 30, 10, 34, 40, 224, DateTimeKind.Local).AddTicks(669),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 30, 10, 55, 37, 692, DateTimeKind.Local).AddTicks(9908));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AbsenceTypeHistory_Guid",
                table: "AbsenceTypeHistory",
                column: "TypeGuid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AbsenceHistory_Guid",
                table: "AbsenceHistory",
                column: "AbsenceGuid");
        }
    }
}
