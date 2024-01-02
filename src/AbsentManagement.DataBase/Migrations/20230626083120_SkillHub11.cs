using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class SkillHub11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceTypeHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 6, 26, 9, 31, 20, 858, DateTimeKind.Local).AddTicks(1255),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 30, 12, 44, 12, 965, DateTimeKind.Local).AddTicks(6082));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 6, 26, 9, 31, 20, 858, DateTimeKind.Local).AddTicks(150),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 30, 12, 44, 12, 965, DateTimeKind.Local).AddTicks(4951));

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "AbsenceHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "None");

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "Absence",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "None");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Desc",
                table: "AbsenceHistory");

            migrationBuilder.DropColumn(
                name: "Desc",
                table: "Absence");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceTypeHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 30, 12, 44, 12, 965, DateTimeKind.Local).AddTicks(6082),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 6, 26, 9, 31, 20, 858, DateTimeKind.Local).AddTicks(1255));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 30, 12, 44, 12, 965, DateTimeKind.Local).AddTicks(4951),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 6, 26, 9, 31, 20, 858, DateTimeKind.Local).AddTicks(150));
        }
    }
}
