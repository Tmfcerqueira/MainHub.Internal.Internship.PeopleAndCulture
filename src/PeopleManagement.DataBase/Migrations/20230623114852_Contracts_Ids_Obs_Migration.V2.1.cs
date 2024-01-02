using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class Contracts_Ids_Obs_MigrationV21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollaboratorId",
                table: "PersonHistory");

            migrationBuilder.DropColumn(
                name: "CollaboratorId",
                table: "Person");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CollaboratorId",
                table: "PersonHistory",
                type: "int",
                maxLength: 5,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CollaboratorId",
                table: "Person",
                type: "int",
                maxLength: 5,
                nullable: false,
                defaultValue: 10000);
        }
    }
}
