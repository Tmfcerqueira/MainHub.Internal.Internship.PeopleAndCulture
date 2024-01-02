using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class Collaborator_HistoryV12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Person",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Person",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PersonHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeopleGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "NoName"),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "No_Adress"),
                    Postal = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "NoPostal_Code"),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "NoCountry"),
                    TaxNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CCNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SSNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CCVal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CivilState = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "NoCivilState"),
                    DependentNum = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    ExitDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "No User"),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    ChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "No Change Date"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Active"),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "None"),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2999, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified)),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Collaborator_CollaboratorId", x => x.Id);
                    table.UniqueConstraint("AK_PersonHistory_CCNumber", x => x.CCNumber);
                    table.UniqueConstraint("AK_PersonHistory_SSNumber", x => x.SSNumber);
                    table.UniqueConstraint("AK_PersonHistory_TaxNumber", x => x.TaxNumber);
                    table.UniqueConstraint("Collaborator_CollaboratorGUID", x => x.PeopleGUID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonHistory");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Person");
        }
    }
}
