using Microsoft.EntityFrameworkCore.Migrations;

namespace KolveniershofBACKEND.Migrations
{
    public partial class Add_BusSchemaV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusColor",
                table: "Driver");

            migrationBuilder.AddColumn<string>(
                name: "BusColor",
                table: "BusDriver",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusColor",
                table: "BusDriver");

            migrationBuilder.AddColumn<string>(
                name: "BusColor",
                table: "Driver",
                nullable: false,
                defaultValue: "");
        }
    }
}
