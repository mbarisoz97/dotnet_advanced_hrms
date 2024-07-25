using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ehrms.TrainingManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class Training_Recommendation_Project_Skill_Relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Project_ProjectId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSkill_Skill_SkillsId",
                table: "EmployeeSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationRequests_Project_ProjectId",
                table: "RecommendationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationResults_Skill_SkillId",
                table: "RecommendationResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Project_ProjectId",
                table: "Skill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skill",
                table: "Skill");

            migrationBuilder.DropIndex(
                name: "IX_Skill_ProjectId",
                table: "Skill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Skill");

            migrationBuilder.RenameTable(
                name: "Skill",
                newName: "Skills");

            migrationBuilder.RenameTable(
                name: "Project",
                newName: "Projects");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skills",
                table: "Skills",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProjectSkill",
                columns: table => new
                {
                    ProjectsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequiredSkillsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSkill", x => new { x.ProjectsId, x.RequiredSkillsId });
                    table.ForeignKey(
                        name: "FK_ProjectSkill_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectSkill_Skills_RequiredSkillsId",
                        column: x => x.RequiredSkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSkill_RequiredSkillsId",
                table: "ProjectSkill",
                column: "RequiredSkillsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Projects_ProjectId",
                table: "Employees",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSkill_Skills_SkillsId",
                table: "EmployeeSkill",
                column: "SkillsId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationRequests_Projects_ProjectId",
                table: "RecommendationRequests",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationResults_Skills_SkillId",
                table: "RecommendationResults",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Projects_ProjectId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSkill_Skills_SkillsId",
                table: "EmployeeSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationRequests_Projects_ProjectId",
                table: "RecommendationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationResults_Skills_SkillId",
                table: "RecommendationResults");

            migrationBuilder.DropTable(
                name: "ProjectSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skills",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.RenameTable(
                name: "Skills",
                newName: "Skill");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Project");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Skill",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skill",
                table: "Skill",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project",
                table: "Project",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_ProjectId",
                table: "Skill",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Project_ProjectId",
                table: "Employees",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSkill_Skill_SkillsId",
                table: "EmployeeSkill",
                column: "SkillsId",
                principalTable: "Skill",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationRequests_Project_ProjectId",
                table: "RecommendationRequests",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationResults_Skill_SkillId",
                table: "RecommendationResults",
                column: "SkillId",
                principalTable: "Skill",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_Project_ProjectId",
                table: "Skill",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");
        }
    }
}
