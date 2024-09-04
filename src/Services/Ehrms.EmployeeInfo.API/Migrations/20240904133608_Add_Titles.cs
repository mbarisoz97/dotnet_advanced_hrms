using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ehrms.EmployeeInfo.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_Titles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TitleId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Titles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TitleId",
                table: "Employees",
                column: "TitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Titles_TitleId",
                table: "Employees",
                column: "TitleId",
                principalTable: "Titles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Titles_TitleId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Titles");

            migrationBuilder.DropIndex(
                name: "IX_Employees_TitleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TitleId",
                table: "Employees");
        }
    }
}
