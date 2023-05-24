using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meetinger.Migrations
{
    public partial class h7iscanceled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Meetings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Meetings");
        }
    }
}
