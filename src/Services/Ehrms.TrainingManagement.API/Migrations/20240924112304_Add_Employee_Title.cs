using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ehrms.TrainingManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_Employee_Title : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TitleId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.DropIndex(
                name: "IX_Employees_TitleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TitleId",
                table: "Employees");
        }
    }
}
