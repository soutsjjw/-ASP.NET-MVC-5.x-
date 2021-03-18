using Microsoft.EntityFrameworkCore.Migrations;

namespace MessageBoard.Data.Migrations
{
    public partial class MemberWithGuestbookRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MemberId",
                table: "Guestbooks",
                type: "nvarchar(25)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Guestbooks_MemberId",
                table: "Guestbooks",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guestbooks_Members_MemberId",
                table: "Guestbooks",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guestbooks_Members_MemberId",
                table: "Guestbooks");

            migrationBuilder.DropIndex(
                name: "IX_Guestbooks_MemberId",
                table: "Guestbooks");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Guestbooks");
        }
    }
}
