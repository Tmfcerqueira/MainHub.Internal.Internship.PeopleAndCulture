using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class Skillhub5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ApprovalStatus",
                table: "Absence",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Draft");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApprovalStatus",
                table: "Absence",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Draft",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);
        }
    }
}
