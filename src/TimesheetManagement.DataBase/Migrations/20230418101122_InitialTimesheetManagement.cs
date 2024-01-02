using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class InitialTimesheetManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: false, defaultValue: "None")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_ProjectId", x => x.ProjectId);
                    table.UniqueConstraint("AK_Project_ProjectGuid", x => x.ProjectGUID);
                });

            migrationBuilder.CreateTable(
                name: "Timesheet",
                columns: table => new
                {
                    TimesheetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimesheetGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Draft"),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "None"),
                    DateOfSubmission = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    DateOfApproval = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timesheet_TimesheetId", x => x.TimesheetId);
                    table.UniqueConstraint("AK_Timesheet_TimesheetGuid", x => x.TimesheetGUID);
                });

            migrationBuilder.CreateTable(
                name: "TimesheetActivity",
                columns: table => new
                {
                    ActivityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimesheetGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeOfWork = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false, defaultValue: "Regular"),
                    ActivityDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    Hours = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesheetActivity_ActivityId", x => x.ActivityId);
                    table.UniqueConstraint("AK_TimesheetActivity_ActivityGuid", x => x.ActivityGUID);
                    table.ForeignKey(
                        name: "FK_TimesheetActivity_ProjectGuid",
                        column: x => x.ProjectGUID,
                        principalTable: "Project",
                        principalColumn: "ProjectGUID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimesheetActivity_TimesheetGuid",
                        column: x => x.TimesheetGUID,
                        principalTable: "Timesheet",
                        principalColumn: "TimesheetGUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetActivity_ProjectGUID",
                table: "TimesheetActivity",
                column: "ProjectGUID");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetActivity_TimesheetGUID",
                table: "TimesheetActivity",
                column: "TimesheetGUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimesheetActivity");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Timesheet");
        }
    }
}
