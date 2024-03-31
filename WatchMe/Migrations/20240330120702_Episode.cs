using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatchMe.Migrations
{
    public partial class Episode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Passive",
                table: "Episodes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ViewCount",
                table: "Episodes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Passive",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Episodes");
        }
    }
}
