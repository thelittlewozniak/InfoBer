using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoBer.Migrations
{
    public partial class rmvTheError : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Users_UserId",
                table: "Problems");

            migrationBuilder.DropIndex(
                name: "IX_Problems_UserId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Problems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Problems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Problems_UserId",
                table: "Problems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_Users_UserId",
                table: "Problems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
