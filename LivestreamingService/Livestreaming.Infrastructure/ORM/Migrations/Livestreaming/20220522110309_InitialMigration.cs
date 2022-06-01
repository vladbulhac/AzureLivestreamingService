using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Livestreaming.Infrastructure.ORM.Migrations.Livestreaming
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Livestreams",
                columns: table => new
                {
                    LivestreamId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LiveStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordingDuration = table.Column<int>(type: "int", nullable: false),
                    LiveEventName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    AssetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LiveOutputName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DrvStreamingLocatorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DrvAssetFilterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArchiveStreamingLocatorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreamingLocatorName = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    StreamingEndpointName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreamingProtocol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EncodingType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IngestUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviewUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HlsManifest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DashManifest = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livestreams", x => x.LivestreamId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Livestreams");
        }
    }
}