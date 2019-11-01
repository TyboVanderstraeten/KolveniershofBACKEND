using Microsoft.EntityFrameworkCore.Migrations;

namespace KolveniershofBACKEND.Migrations
{
    public partial class SplitUpMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeekendDay_User_UserId",
                table: "WeekendDay");

            migrationBuilder.RenameColumn(
                name: "Menu",
                table: "Day",
                newName: "PreDish");

            migrationBuilder.AddColumn<string>(
                name: "Dessert",
                table: "Day",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainDish",
                table: "Day",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WeekendDay_User_UserId",
                table: "WeekendDay",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeekendDay_User_UserId",
                table: "WeekendDay");

            migrationBuilder.DropColumn(
                name: "Dessert",
                table: "Day");

            migrationBuilder.DropColumn(
                name: "MainDish",
                table: "Day");

            migrationBuilder.RenameColumn(
                name: "PreDish",
                table: "Day",
                newName: "Menu");

            migrationBuilder.AddForeignKey(
                name: "FK_WeekendDay_User_UserId",
                table: "WeekendDay",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
