using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainHub.Internal.PeopleAndCulture.ProjectManagement.Data.Migrations
{
    public partial class ProjecManagement_Basic_Initial_Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "No Project Ativity")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectActivity_Id", x => x.Id);
                    table.UniqueConstraint("AK_ProjectActivity_ProjectActivityId", x => x.ProjectActivityId);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "No Project")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Id", x => x.Id);
                    table.UniqueConstraint("AK_Project_ProjectId", x => x.ProjectId);
                });

            migrationBuilder.InsertData(
                table: "ProjectActivities",
                columns: new[] { "Id", "Name", "ProjectActivityId" },
                values: new object[,]
                {
                    { 1, "Desenvolvimento", new Guid("ff438220-eada-4ef3-c5f3-08db73d5eeec") },
                    { 2, "Análise Processo", new Guid("badfc849-a3f5-4136-c5f4-08db73d5eeec") },
                    { 3, "Análise Técnica", new Guid("eb8e83e1-0ffd-4d83-c5f5-08db73d5eeec") },
                    { 4, "Documentação", new Guid("a7e3a11c-ec7b-4cb3-c5f6-08db73d5eeec") },
                    { 5, "Formação", new Guid("383cb601-c9af-4c83-c5f7-08db73d5eeec") },
                    { 6, "Reuniões", new Guid("2c68d1c2-2740-4cc9-c5f8-08db73d5eeec") },
                    { 7, "Férias", new Guid("a3f226f1-d276-4651-c5f9-08db73d5eeec") },
                    { 8, "Consulta Médica", new Guid("ec0dab74-be39-4c6d-c5fa-08db73d5eeec") },
                    { 9, "Outro", new Guid("4467d2cb-29a8-4f23-c5fb-08db73d5eeec") }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Name", "ProjectId" },
                values: new object[,]
                {
                    { 1, "Singular Data", new Guid("ea07b055-f15b-4a5e-61b3-08db73d5eee9") },
                    { 2, "GS Tyres", new Guid("3bbe64da-a320-4aec-61b4-08db73d5eee9") },
                    { 3, "Flumen", new Guid("7c0878a6-aee5-401a-61b5-08db73d5eee9") },
                    { 4, "Grupo Pestana", new Guid("49d29cb4-6961-4e63-61b6-08db73d5eee9") },
                    { 5, "CCP", new Guid("a28846e0-49f9-4f46-61b7-08db73d5eee9") },
                    { 6, "Milestone", new Guid("65c8d9ac-3d13-466a-61b8-08db73d5eee9") },
                    { 7, "AdvanceCare", new Guid("9976cc3b-2969-4994-61b9-08db73d5eee9") },
                    { 8, "EY - Turismo de Portugal", new Guid("b63be6eb-fa5e-4ce7-61ba-08db73d5eee9") },
                    { 9, "EY - Abreu Advogados", new Guid("61302d7c-c3c9-4c3f-61bb-08db73d5eee9") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectActivities");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
