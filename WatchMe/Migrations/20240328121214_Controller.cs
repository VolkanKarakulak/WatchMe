using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatchMe.Migrations
{
    public partial class Controller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Episodes_MediaId",
                table: "Episodes");

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_MediaId_SeasonNumber_EpisodeNumber",
                table: "Episodes",
                columns: new[] { "MediaId", "SeasonNumber", "EpisodeNumber" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Episodes_MediaId_SeasonNumber_EpisodeNumber",
                table: "Episodes");

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_MediaId",
                table: "Episodes",
                column: "MediaId");
        }
    }
}
