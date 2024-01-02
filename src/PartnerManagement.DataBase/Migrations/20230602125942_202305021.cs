using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class _202305021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PartnerHistory_Guid",
                table: "Partner_History");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ContactHistory_Guid",
                table: "Contact_History");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_PartnerHistory_Guid",
                table: "Partner_History",
                column: "PartnerGUID");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ContactHistory_Guid",
                table: "Contact_History",
                column: "ContactGUID");
        }
    }
}
