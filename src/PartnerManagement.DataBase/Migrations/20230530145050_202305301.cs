using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class _202305301 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Partner",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Partner",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Contact",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Contact",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Contact_History",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartnerGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactHistory_ID", x => x.Id);
                    table.UniqueConstraint("AK_ContactHistory_Guid", x => x.ContactGUID);
                });

            migrationBuilder.CreateTable(
                name: "Partner_History",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartnerGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Locality = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TaxNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServiceDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerHistory_ID", x => x.Id);
                    table.UniqueConstraint("AK_PartnerHistory_Guid", x => x.PartnerGUID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact_History");

            migrationBuilder.DropTable(
                name: "Partner_History");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Contact");
        }
    }
}
