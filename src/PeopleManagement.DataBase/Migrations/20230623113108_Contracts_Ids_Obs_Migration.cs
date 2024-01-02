using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class Contracts_Ids_Obs_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CollaboratorId",
                table: "PersonHistory",
                type: "int",
                maxLength: 5,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContractType",
                table: "PersonHistory",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CollaboratorId",
                table: "Person",
                type: "int",
                maxLength: 5,
                nullable: false,
                defaultValue: 10000);

            migrationBuilder.AddColumn<int>(
                name: "ContractType",
                table: "Person",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollaboratorId",
                table: "PersonHistory");

            migrationBuilder.DropColumn(
                name: "ContractType",
                table: "PersonHistory");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "PersonHistory");

            migrationBuilder.DropColumn(
                name: "CollaboratorId",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "ContractType",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "Person");
        }
    }
}
