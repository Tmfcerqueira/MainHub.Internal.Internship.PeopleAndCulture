using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class TimesheetIsDeletedV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IsDeleted",
                table: "TimesheetActivity",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "IsDeleted",
                table: "Timesheet",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TimesheetActivity",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Timesheet",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);
        }
    }
}
