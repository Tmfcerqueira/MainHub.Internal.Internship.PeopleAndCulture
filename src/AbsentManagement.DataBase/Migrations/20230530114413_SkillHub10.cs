using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class SkillHub10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceTypeHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 30, 12, 44, 12, 965, DateTimeKind.Local).AddTicks(6082),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 30, 11, 11, 17, 355, DateTimeKind.Local).AddTicks(5706));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "AbsenceType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "AbsenceType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "AbsenceType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedBy",
                table: "AbsenceHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "None",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 30, 12, 44, 12, 965, DateTimeKind.Local).AddTicks(4951),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 30, 11, 11, 17, 355, DateTimeKind.Local).AddTicks(4718));

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedBy",
                table: "Absence",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "None",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Absence",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Absence",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Absence",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AbsenceType");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AbsenceType");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "AbsenceType");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Absence");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Absence");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Absence");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceTypeHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 30, 11, 11, 17, 355, DateTimeKind.Local).AddTicks(5706),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 30, 12, 44, 12, 965, DateTimeKind.Local).AddTicks(6082));

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedBy",
                table: "AbsenceHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "None");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 30, 11, 11, 17, 355, DateTimeKind.Local).AddTicks(4718),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 30, 12, 44, 12, 965, DateTimeKind.Local).AddTicks(4951));

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedBy",
                table: "Absence",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "None");
        }
    }
}
