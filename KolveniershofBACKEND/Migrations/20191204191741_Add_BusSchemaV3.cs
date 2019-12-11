using Microsoft.EntityFrameworkCore.Migrations;

namespace KolveniershofBACKEND.Migrations
{
    public partial class Add_BusSchemaV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BusDriver",
                table: "BusDriver");

            migrationBuilder.AlterColumn<string>(
                name: "BusColor",
                table: "BusDriver",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusDriver",
                table: "BusDriver",
                columns: new[] { "DayId", "BusColor", "TimeOfDay" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BusDriver",
                table: "BusDriver");

            migrationBuilder.AlterColumn<string>(
                name: "BusColor",
                table: "BusDriver",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusDriver",
                table: "BusDriver",
                columns: new[] { "DayId", "DriverId", "TimeOfDay" });
        }
    }
}
