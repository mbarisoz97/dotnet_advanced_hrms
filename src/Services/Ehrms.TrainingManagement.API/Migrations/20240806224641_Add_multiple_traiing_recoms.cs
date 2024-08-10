using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ehrms.TrainingManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_multiple_traiing_recoms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_RecommendationResults_TrainingRecommendationResultId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_TrainingRecommendationResultId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TrainingRecommendationResultId",
                table: "Employees");

            migrationBuilder.CreateTable(
                name: "EmployeeTrainingRecommendationResult",
                columns: table => new
                {
                    EmployeesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainingRecommendationsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTrainingRecommendationResult", x => new { x.EmployeesId, x.TrainingRecommendationsId });
                    table.ForeignKey(
                        name: "FK_EmployeeTrainingRecommendationResult_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeTrainingRecommendationResult_RecommendationResults_TrainingRecommendationsId",
                        column: x => x.TrainingRecommendationsId,
                        principalTable: "RecommendationResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTrainingRecommendationResult_TrainingRecommendationsId",
                table: "EmployeeTrainingRecommendationResult",
                column: "TrainingRecommendationsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeTrainingRecommendationResult");

            migrationBuilder.AddColumn<Guid>(
                name: "TrainingRecommendationResultId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TrainingRecommendationResultId",
                table: "Employees",
                column: "TrainingRecommendationResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_RecommendationResults_TrainingRecommendationResultId",
                table: "Employees",
                column: "TrainingRecommendationResultId",
                principalTable: "RecommendationResults",
                principalColumn: "Id");
        }
    }
}
