using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class UpdatedTimesheetManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_TimesheetActivity_ActivityGuid",
                table: "TimesheetActivity");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "TimesheetActivity",
                newName: "TimesheetActivityId");

            migrationBuilder.AlterColumn<int>(
                name: "TypeOfWork",
                table: "TimesheetActivity",
                type: "int",
                maxLength: 32,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32,
                oldDefaultValue: "Regular");

            migrationBuilder.AddColumn<Guid>(
                name: "TimesheetActivityGUID",
                table: "TimesheetActivity",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "ApprovalStatus",
                table: "Timesheet",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Draft");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Timesheet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TimesheetActivity_ActivityGuid",
                table: "TimesheetActivity",
                column: "TimesheetActivityGUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_TimesheetActivity_ActivityGuid",
                table: "TimesheetActivity");

            migrationBuilder.DropColumn(
                name: "TimesheetActivityGUID",
                table: "TimesheetActivity");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Timesheet");

            migrationBuilder.RenameColumn(
                name: "TimesheetActivityId",
                table: "TimesheetActivity",
                newName: "ActivityId");

            migrationBuilder.AlterColumn<string>(
                name: "TypeOfWork",
                table: "TimesheetActivity",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "Regular",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 32,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovalStatus",
                table: "Timesheet",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Draft",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50,
                oldDefaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TimesheetActivity_ActivityGuid",
                table: "TimesheetActivity",
                column: "ActivityGUID");
        }
    }
}
