using Microsoft.EntityFrameworkCore.Migrations;

namespace LoveLife.API.Migrations
{
    public partial class PublicID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicID",
                table: "Photo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicID",
                table: "Photo");
        }
    }
}
