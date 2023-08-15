using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakaMangaAPI.Data.Migrations
{
    public partial class ConfigureViewUniqueness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Views_ChapterId",
                table: "Views");

            migrationBuilder.DropIndex(
                name: "IX_Views_PostId",
                table: "Views");

            migrationBuilder.CreateIndex(
                name: "IX_Views_ChapterId_UserId",
                table: "Views",
                columns: new[] { "ChapterId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Views_PostId_UserId",
                table: "Views",
                columns: new[] { "PostId", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Views_ChapterId_UserId",
                table: "Views");

            migrationBuilder.DropIndex(
                name: "IX_Views_PostId_UserId",
                table: "Views");

            migrationBuilder.CreateIndex(
                name: "IX_Views_ChapterId",
                table: "Views",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Views_PostId",
                table: "Views",
                column: "PostId");
        }
    }
}
