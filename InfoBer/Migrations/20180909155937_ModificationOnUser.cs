using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoBer.Migrations
{
    public partial class ModificationOnUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TakerId",
                table: "Problems");

            migrationBuilder.RenameColumn(
                name: "WriterId",
                table: "Problems",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Problems_WriterId",
                table: "Problems",
                newName: "IX_Problems_UserId");

            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Result = table.Column<double>(nullable: false),
                    Commentary = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RatingId",
                table: "Users",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_Users_UserId",
                table: "Problems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Rating_RatingId",
                table: "Users",
                column: "RatingId",
                principalTable: "Rating",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Users_UserId",
                table: "Problems");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Rating_RatingId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Users_RatingId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Problems",
                newName: "WriterId");

            migrationBuilder.RenameIndex(
                name: "IX_Problems_UserId",
                table: "Problems",
                newName: "IX_Problems_WriterId");

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Users",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TakerId",
                table: "Problems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Problems_TakerId",
                table: "Problems",
                column: "TakerId");

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
    }
}
