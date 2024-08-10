using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ehrms.TrainingManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class Training_Recommendation_Models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TrainingRecommendationResultId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecommendationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestStatus = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendationRequests_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skill_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSkill",
                columns: table => new
                {
                    EmployeesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkill", x => new { x.EmployeesId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_EmployeeSkill_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeSkill_Skill_SkillsId",
                        column: x => x.SkillsId,
                        principalTable: "Skill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecommendationResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecommendationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendationResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendationResults_RecommendationRequests_RecommendationRequestId",
                        column: x => x.RecommendationRequestId,
                        principalTable: "RecommendationRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecommendationResults_Skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skill",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ProjectId",
                table: "Employees",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TrainingRecommendationResultId",
                table: "Employees",
                column: "TrainingRecommendationResultId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkill_SkillsId",
                table: "EmployeeSkill",
                column: "SkillsId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationRequests_ProjectId",
                table: "RecommendationRequests",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationResults_RecommendationRequestId",
                table: "RecommendationResults",
                column: "RecommendationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationResults_SkillId",
                table: "RecommendationResults",
                column: "SkillId");

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
                name: "FK_Employees_RecommendationResults_TrainingRecommendationResultId",
                table: "Employees",
                column: "TrainingRecommendationResultId",
                principalTable: "RecommendationResults",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Project_ProjectId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_RecommendationResults_TrainingRecommendationResultId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "EmployeeSkill");

            migrationBuilder.DropTable(
                name: "RecommendationResults");

            migrationBuilder.DropTable(
                name: "RecommendationRequests");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ProjectId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_TrainingRecommendationResultId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TrainingRecommendationResultId",
                table: "Employees");
        }
    }
}
