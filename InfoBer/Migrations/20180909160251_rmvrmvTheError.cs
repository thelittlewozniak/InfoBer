using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoBer.Migrations
{
    public partial class rmvrmvTheError : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TakerId",
                table: "Problems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WriterId",
                table: "Problems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Problems_TakerId",
                table: "Problems",
                column: "TakerId");

            migrationBuilder.CreateIndex(
                name: "IX_Problems_WriterId",
                table: "Problems",
                column: "WriterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_Users_TakerId",
                table: "Problems",
                column: "TakerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_Users_WriterId",
                table: "Problems",
                column: "WriterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Users_TakerId",
                table: "Problems");

            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Users_WriterId",
                table: "Problems");

            migrationBuilder.DropIndex(
                name: "IX_Problems_TakerId",
                table: "Problems");

            migrationBuilder.DropIndex(
                name: "IX_Problems_WriterId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "TakerId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "WriterId",
                table: "Problems");
        }
    }
}
