using Microsoft.EntityFrameworkCore.Migrations;

namespace MessageBoard.Data.Migrations
{
    public partial class AddReplierColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReplierId",
                table: "Guestbooks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guestbooks_ReplierId",
                table: "Guestbooks",
                column: "ReplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guestbooks_UserDatas_ReplierId",
                table: "Guestbooks",
                column: "ReplierId",
                principalTable: "UserDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guestbooks_UserDatas_ReplierId",
                table: "Guestbooks");

            migrationBuilder.DropIndex(
                name: "IX_Guestbooks_ReplierId",
                table: "Guestbooks");

            migrationBuilder.DropColumn(
                name: "ReplierId",
                table: "Guestbooks");
        }
    }
}
