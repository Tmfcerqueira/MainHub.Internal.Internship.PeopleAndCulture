using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class ContactsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "PersonHistory",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContact",
                table: "PersonHistory",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Person",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContact",
                table: "Person",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact",
                table: "PersonHistory");

            migrationBuilder.DropColumn(
                name: "EmergencyContact",
                table: "PersonHistory");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "EmergencyContact",
                table: "Person");
        }
    }
}
