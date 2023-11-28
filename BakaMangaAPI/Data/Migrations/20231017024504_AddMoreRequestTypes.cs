using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakaMangaAPI.Data.Migrations
{
    public partial class AddMoreRequestTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Requests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MangaAuthor",
                table: "Requests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MangaSource",
                table: "Requests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MangaTitle",
                table: "Requests",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Requests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "MangaAuthor",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "MangaSource",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "MangaTitle",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Requests");
        }
    }
}
