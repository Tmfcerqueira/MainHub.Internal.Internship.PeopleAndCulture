using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.Migrations
{
    public partial class SkillHub8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absence_AbsenceTypeHistory_AbsenceTypeHistoryId",
                table: "Absence");

            migrationBuilder.DropForeignKey(
                name: "FK_AbsenceHistory_AbsenceType_AbsenceTypeId",
                table: "AbsenceHistory");

            migrationBuilder.DropIndex(
                name: "IX_AbsenceHistory_AbsenceTypeId",
                table: "AbsenceHistory");

            migrationBuilder.DropIndex(
                name: "IX_Absence_AbsenceTypeHistoryId",
                table: "Absence");

            migrationBuilder.DropColumn(
                name: "AbsenceTypeId",
                table: "AbsenceHistory");

            migrationBuilder.DropColumn(
                name: "AbsenceTypeHistoryId",
                table: "Absence");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceTypeHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 30, 11, 11, 17, 355, DateTimeKind.Local).AddTicks(5706),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 30, 10, 55, 37, 693, DateTimeKind.Local).AddTicks(743));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "AbsenceTypeHistory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 30, 11, 11, 17, 355, DateTimeKind.Local).AddTicks(4718),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 30, 10, 55, 37, 692, DateTimeKind.Local).AddTicks(9908));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "AbsenceHistory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AbsenceTypeHistory");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AbsenceHistory");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceTypeHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 30, 10, 55, 37, 693, DateTimeKind.Local).AddTicks(743),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 30, 11, 11, 17, 355, DateTimeKind.Local).AddTicks(5706));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDate",
                table: "AbsenceHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 30, 10, 55, 37, 692, DateTimeKind.Local).AddTicks(9908),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 30, 11, 11, 17, 355, DateTimeKind.Local).AddTicks(4718));

            migrationBuilder.AddColumn<int>(
                name: "AbsenceTypeId",
                table: "AbsenceHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AbsenceTypeHistoryId",
                table: "Absence",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbsenceHistory_AbsenceTypeId",
                table: "AbsenceHistory",
                column: "AbsenceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Absence_AbsenceTypeHistoryId",
                table: "Absence",
                column: "AbsenceTypeHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_AbsenceTypeHistory_AbsenceTypeHistoryId",
                table: "Absence",
                column: "AbsenceTypeHistoryId",
                principalTable: "AbsenceTypeHistory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AbsenceHistory_AbsenceType_AbsenceTypeId",
                table: "AbsenceHistory",
                column: "AbsenceTypeId",
                principalTable: "AbsenceType",
                principalColumn: "Id");
        }
    }
}
