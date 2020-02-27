using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace youtube_xml_dotnet.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "videos",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    views = table.Column<uint>(nullable: true),
                    uploadDate = table.Column<DateTime>(nullable: false),
                    thumbnail = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_videos", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "videos");
        }
    }
}
