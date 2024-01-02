using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class TimesheetHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TypeOfWork",
                table: "TimesheetActivity",
                type: "int",
                maxLength: 32,
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 32,
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TimesheetActivity",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "ApprovalStatus",
                table: "Timesheet",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50,
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Timesheet",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TimesheetActivityHistory",
                columns: table => new
                {
                    TimesheetActivityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimesheetActivityGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimesheetGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeOfWork = table.Column<int>(type: "int", maxLength: 32, nullable: false, defaultValue: 1),
                    ActivityDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    Hours = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Action = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false, defaultValue: "None"),
                    ActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesheetActivityHistory_ActivityId", x => x.TimesheetActivityId);
                    table.UniqueConstraint("AK_TimesheetActivityHistory_ActivityGuid", x => x.TimesheetActivityGUID);
                });

            migrationBuilder.CreateTable(
                name: "TimesheetHistory",
                columns: table => new
                {
                    TimesheetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimesheetGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Year = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ApprovalStatus = table.Column<int>(type: "int", maxLength: 50, nullable: false, defaultValue: 1),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "None"),
                    DateOfSubmission = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    DateOfApproval = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    Action = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false, defaultValue: "None"),
                    ActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesheetHistory_TimesheetId", x => x.TimesheetId);
                    table.UniqueConstraint("AK_TimesheetHistory_TimesheetGuid", x => x.TimesheetGUID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimesheetActivityHistory");

            migrationBuilder.DropTable(
                name: "TimesheetHistory");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TimesheetActivity");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Timesheet");

            migrationBuilder.AlterColumn<int>(
                name: "TypeOfWork",
                table: "TimesheetActivity",
                type: "int",
                maxLength: 32,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 32,
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "ApprovalStatus",
                table: "Timesheet",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50,
                oldDefaultValue: 1);
        }
    }
}
