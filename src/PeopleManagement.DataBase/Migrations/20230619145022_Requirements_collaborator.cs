using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class Requirements_collaborator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Person_CCNumber",
                table: "Person");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Person_SSNumber",
                table: "Person");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Person_TaxNumber",
                table: "Person");

            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "Active",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Active");

            migrationBuilder.AlterColumn<string>(
                name: "SSNumber",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Postal",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "NoPostal_Code",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "NoPostal_Code");

            migrationBuilder.AlterColumn<string>(
                name: "Locality",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "NoPostal_Code",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "NoPostal_Code");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "No User",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "No User");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "NoCountry",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "NoCountry");

            migrationBuilder.AlterColumn<string>(
                name: "CivilState",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "NoCivilState",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "NoCivilState");

            migrationBuilder.AlterColumn<string>(
                name: "ChangedBy",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "No Change Date",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "No Change Date");

            migrationBuilder.AlterColumn<string>(
                name: "CCNumber",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Adress",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "No_Adress",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "No_Adress");

            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "Active",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Active");

            migrationBuilder.AlterColumn<string>(
                name: "SSNumber",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Postal",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "NoPostal_Code",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "NoPostal_Code");

            migrationBuilder.AlterColumn<string>(
                name: "Locality",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "NoPostal_Code",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "NoPostal_Code");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "No User",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "No User");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "NoCountry",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "NoCountry");

            migrationBuilder.AlterColumn<string>(
                name: "CivilState",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "NoCivilState",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "NoCivilState");

            migrationBuilder.AlterColumn<string>(
                name: "ChangedBy",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "No Change Date",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "No Change Date");

            migrationBuilder.AlterColumn<string>(
                name: "CCNumber",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Adress",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "No_Adress",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "No_Adress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Active",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "Active");

            migrationBuilder.AlterColumn<string>(
                name: "SSNumber",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Postal",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "NoPostal_Code",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "NoPostal_Code");

            migrationBuilder.AlterColumn<string>(
                name: "Locality",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "NoPostal_Code",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "NoPostal_Code");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "No User",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "No User");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "NoCountry",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "NoCountry");

            migrationBuilder.AlterColumn<string>(
                name: "CivilState",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "NoCivilState",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "NoCivilState");

            migrationBuilder.AlterColumn<string>(
                name: "ChangedBy",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "No Change Date",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "No Change Date");

            migrationBuilder.AlterColumn<string>(
                name: "CCNumber",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Adress",
                table: "PersonHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "No_Adress",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "No_Adress");

            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "Person",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Person",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Active",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "Active");

            migrationBuilder.AlterColumn<string>(
                name: "SSNumber",
                table: "Person",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Postal",
                table: "Person",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "NoPostal_Code",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "NoPostal_Code");

            migrationBuilder.AlterColumn<string>(
                name: "Locality",
                table: "Person",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "NoPostal_Code",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "NoPostal_Code");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Person",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "No User",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "No User");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Person",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "NoCountry",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "NoCountry");

            migrationBuilder.AlterColumn<string>(
                name: "CivilState",
                table: "Person",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "NoCivilState",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "NoCivilState");

            migrationBuilder.AlterColumn<string>(
                name: "ChangedBy",
                table: "Person",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "No Change Date",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "No Change Date");

            migrationBuilder.AlterColumn<string>(
                name: "CCNumber",
                table: "Person",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Adress",
                table: "Person",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "No_Adress",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "No_Adress");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Person_CCNumber",
                table: "Person",
                column: "CCNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Person_SSNumber",
                table: "Person",
                column: "SSNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Person_TaxNumber",
                table: "Person",
                column: "TaxNumber");
        }
    }
}
