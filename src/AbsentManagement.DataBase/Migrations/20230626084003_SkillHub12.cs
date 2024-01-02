using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class SkillHub12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Desc",
                table: "AbsenceHistory",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Desc",
                table: "Absence",
                newName: "Description");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceTypeHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 6, 26, 9, 40, 3, 44, DateTimeKind.Local).AddTicks(6009),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 6, 26, 9, 31, 20, 858, DateTimeKind.Local).AddTicks(1255));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 6, 26, 9, 40, 3, 44, DateTimeKind.Local).AddTicks(4919),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 6, 26, 9, 31, 20, 858, DateTimeKind.Local).AddTicks(150));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "AbsenceHistory",
                newName: "Desc");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Absence",
                newName: "Desc");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceTypeHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 6, 26, 9, 31, 20, 858, DateTimeKind.Local).AddTicks(1255),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 6, 26, 9, 40, 3, 44, DateTimeKind.Local).AddTicks(6009));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 6, 26, 9, 31, 20, 858, DateTimeKind.Local).AddTicks(150),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 6, 26, 9, 40, 3, 44, DateTimeKind.Local).AddTicks(4919));
        }
    }
}
