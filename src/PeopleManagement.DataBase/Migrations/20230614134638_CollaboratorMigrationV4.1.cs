using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class CollaboratorMigrationV41 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PersonHistory",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Person",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "noemail@mainhub.pt");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "NoName");

            migrationBuilder.AddColumn<string>(
                name: "Locality",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "NoPostal_Code");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Person",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "noemail@mainhub.pt");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Person",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "NoName");

            migrationBuilder.AddColumn<string>(
                name: "Locality",
                table: "Person",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "NoPostal_Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "PersonHistory");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "PersonHistory");

            migrationBuilder.DropColumn(
                name: "Locality",
                table: "PersonHistory");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Locality",
                table: "Person");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "PersonHistory",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Person",
                newName: "Name");
        }
    }
}
