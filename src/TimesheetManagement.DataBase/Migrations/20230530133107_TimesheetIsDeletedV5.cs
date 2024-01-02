using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class TimesheetIsDeletedV5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "TimesheetActivity",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "TimesheetActivity",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EditedBy",
                table: "TimesheetActivity",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Timesheet",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Timesheet",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EditedBy",
                table: "Timesheet",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TimesheetActivity");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "TimesheetActivity");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "TimesheetActivity");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Timesheet");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Timesheet");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "Timesheet");
        }
    }
}
