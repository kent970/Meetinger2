using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meetinger.Migrations
{
    public partial class h6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AttendanceStatus",
                table: "Participants",
                type: "bit",
                nullable: false,
                defaultValue: false);

        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
