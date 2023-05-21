using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meetinger.Migrations
{
    public partial class h2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingParticipant_AspNetUsers_ApplicationUserId",
                table: "MeetingParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_MeetingParticipant_Meetings_MeetingId",
                table: "MeetingParticipant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingParticipant",
                table: "MeetingParticipant");

            migrationBuilder.RenameTable(
                name: "MeetingParticipant",
                newName: "Participants");

            migrationBuilder.RenameIndex(
                name: "IX_MeetingParticipant_MeetingId",
                table: "Participants",
                newName: "IX_Participants_MeetingId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Participants",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ParticipantId",
                table: "Participants",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participants",
                table: "Participants",
                columns: new[] { "ParticipantId", "MeetingId" });

            migrationBuilder.CreateIndex(
                name: "IX_Participants_ApplicationUserId",
                table: "Participants",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_AspNetUsers_ApplicationUserId",
                table: "Participants",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Meetings_MeetingId",
                table: "Participants",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_AspNetUsers_ApplicationUserId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Meetings_MeetingId",
                table: "Participants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participants",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_ApplicationUserId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "ParticipantId",
                table: "Participants");

            migrationBuilder.RenameTable(
                name: "Participants",
                newName: "MeetingParticipant");

            migrationBuilder.RenameIndex(
                name: "IX_Participants_MeetingId",
                table: "MeetingParticipant",
                newName: "IX_MeetingParticipant_MeetingId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "MeetingParticipant",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingParticipant",
                table: "MeetingParticipant",
                columns: new[] { "ApplicationUserId", "MeetingId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingParticipant_AspNetUsers_ApplicationUserId",
                table: "MeetingParticipant",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingParticipant_Meetings_MeetingId",
                table: "MeetingParticipant",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id");
        }
    }
}
