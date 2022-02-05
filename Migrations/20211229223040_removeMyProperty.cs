using Microsoft.EntityFrameworkCore.Migrations;

namespace libry.Migrations
{
    public partial class removeMyProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Books");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
