using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoBer.Migrations
{
    public partial class AddingIdChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdChat",
                table: "Problems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdChat",
                table: "Problems");
        }
    }
}
