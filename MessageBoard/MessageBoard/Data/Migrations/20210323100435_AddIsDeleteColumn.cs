using Microsoft.EntityFrameworkCore.Migrations;

namespace MessageBoard.Migrations.MessageBoard
{
    public partial class AddIsDeleteColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Guestbooks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Guestbooks");
        }
    }
}
