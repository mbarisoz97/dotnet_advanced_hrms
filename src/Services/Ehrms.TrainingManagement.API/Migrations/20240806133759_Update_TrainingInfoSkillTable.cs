using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ehrms.TrainingManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class Update_TrainingInfoSkillTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SkillName",
                table: "Skills",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Skills",
                newName: "SkillName");
        }
    }
}
