using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dot.Infrastructure.Migrations
{
    public partial class SchoolData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Schools",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Schools");
        }
    }
}
