using Microsoft.EntityFrameworkCore.Migrations;

namespace KolveniershofBACKEND.Migrations
{
    public partial class Add_TemplateName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TemplateName",
                table: "Day",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateName",
                table: "Day");
        }
    }
}
