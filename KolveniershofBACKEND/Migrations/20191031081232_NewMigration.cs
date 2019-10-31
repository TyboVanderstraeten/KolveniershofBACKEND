using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KolveniershofBACKEND.Migrations
{
    public partial class NewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Day_User_UserId",
                table: "Day");

            migrationBuilder.DropForeignKey(
                name: "FK_Note_Day_CustomDayDayId",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Note_CustomDayDayId",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Day_UserId",
                table: "Day");

            migrationBuilder.DropColumn(
                name: "CustomDayDayId",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Day");

            migrationBuilder.DropColumn(
                name: "CustomWeekendDay_Date",
                table: "Day");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Day");

            migrationBuilder.AddColumn<int>(
                name: "DayId",
                table: "Note",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WeekendDay",
                columns: table => new
                {
                    WeekendDayId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Comment = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekendDay", x => x.WeekendDayId);
                    table.ForeignKey(
                        name: "FK_WeekendDay_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Note_DayId",
                table: "Note",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_WeekendDay_UserId",
                table: "WeekendDay",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Day_DayId",
                table: "Note",
                column: "DayId",
                principalTable: "Day",
                principalColumn: "DayId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_Day_DayId",
                table: "Note");

            migrationBuilder.DropTable(
                name: "WeekendDay");

            migrationBuilder.DropIndex(
                name: "IX_Note_DayId",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "DayId",
                table: "Note");

            migrationBuilder.AddColumn<int>(
                name: "CustomDayDayId",
                table: "Note",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Day",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CustomWeekendDay_Date",
                table: "Day",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Day",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Note_CustomDayDayId",
                table: "Note",
                column: "CustomDayDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Day_UserId",
                table: "Day",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Day_User_UserId",
                table: "Day",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Day_CustomDayDayId",
                table: "Note",
                column: "CustomDayDayId",
                principalTable: "Day",
                principalColumn: "DayId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
