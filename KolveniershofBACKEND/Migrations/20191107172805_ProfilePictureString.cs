using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KolveniershofBACKEND.Migrations
{
    public partial class ProfilePictureString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicture",
                table: "User",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "Binary(16)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePicture",
                table: "User",
                type: "Binary(16)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
