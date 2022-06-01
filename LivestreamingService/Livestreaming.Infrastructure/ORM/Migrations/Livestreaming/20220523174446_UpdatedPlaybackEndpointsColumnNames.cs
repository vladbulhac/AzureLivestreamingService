using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Livestreaming.Infrastructure.ORM.Migrations.Livestreaming
{
    public partial class UpdatedPlaybackEndpointsColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlaybackDashManifest",
                table: "Livestreams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlaybackHlsManifest",
                table: "Livestreams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlaybackDashManifest",
                table: "Livestreams");

            migrationBuilder.DropColumn(
                name: "PlaybackHlsManifest",
                table: "Livestreams");
        }
    }
}