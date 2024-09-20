using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ehrms.TrainingManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainingRecommendationPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingRecommendationPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TitleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingRecommendationPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingRecommendationPreferences_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingRecommendationPreferences_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SkillTrainingRecommendationPreferences",
                columns: table => new
                {
                    SkillsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainingRecommendationPreferencesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillTrainingRecommendationPreferences", x => new { x.SkillsId, x.TrainingRecommendationPreferencesId });
                    table.ForeignKey(
                        name: "FK_SkillTrainingRecommendationPreferences_Skills_SkillsId",
                        column: x => x.SkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkillTrainingRecommendationPreferences_TrainingRecommendationPreferences_TrainingRecommendationPreferencesId",
                        column: x => x.TrainingRecommendationPreferencesId,
                        principalTable: "TrainingRecommendationPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillTrainingRecommendationPreferences_TrainingRecommendationPreferencesId",
                table: "SkillTrainingRecommendationPreferences",
                column: "TrainingRecommendationPreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingRecommendationPreferences_ProjectId",
                table: "TrainingRecommendationPreferences",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingRecommendationPreferences_TitleId",
                table: "TrainingRecommendationPreferences",
                column: "TitleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkillTrainingRecommendationPreferences");

            migrationBuilder.DropTable(
                name: "TrainingRecommendationPreferences");
        }
    }
}
