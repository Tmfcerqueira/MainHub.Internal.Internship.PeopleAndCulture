using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class Employee_Id_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PersonHistory",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDate",
                table: "PersonHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "No User",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CCVal",
                table: "PersonHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "Employee_Id",
                table: "PersonHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Employee_Id",
                table: "Person",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Employee_Id",
                table: "PersonHistory");

            migrationBuilder.DropColumn(
                name: "Employee_Id",
                table: "Person");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PersonHistory",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedDate",
                table: "PersonHistory",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "No User");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CCVal",
                table: "PersonHistory",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified));
        }
    }
}
