using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoBer.Migrations
{
    public partial class UpdateShoolDepartemetUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Departement",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "DepartementId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartementId",
                table: "Users",
                column: "DepartementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departements_DepartementId",
                table: "Users",
                column: "DepartementId",
                principalTable: "Departements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departements_DepartementId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DepartementId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DepartementId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Departement",
                table: "Users",
                nullable: true);
        }
    }
}
