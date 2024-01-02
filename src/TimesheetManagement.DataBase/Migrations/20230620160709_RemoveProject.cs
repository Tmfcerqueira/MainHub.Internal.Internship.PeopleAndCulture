using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class RemoveProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetActivity_ProjectGuid",
                table: "TimesheetActivity");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_TimesheetHistory_TimesheetGuid",
                table: "TimesheetHistory");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_TimesheetActivityHistory_ActivityGuid",
                table: "TimesheetActivityHistory");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "TimesheetActivity",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetActivity_ProjectId",
                table: "TimesheetActivity",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetActivity_Project_ProjectId",
                table: "TimesheetActivity",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetActivity_Project_ProjectId",
                table: "TimesheetActivity");

            migrationBuilder.DropIndex(
                name: "IX_TimesheetActivity_ProjectId",
                table: "TimesheetActivity");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "TimesheetActivity");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TimesheetHistory_TimesheetGuid",
                table: "TimesheetHistory",
                column: "TimesheetGUID");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TimesheetActivityHistory_ActivityGuid",
                table: "TimesheetActivityHistory",
                column: "TimesheetActivityGUID");

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetActivity_ProjectGuid",
                table: "TimesheetActivity",
                column: "ProjectGUID",
                principalTable: "Project",
                principalColumn: "ProjectGUID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
