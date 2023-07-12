using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakaMangaAPI.Data.Migrations
{
    public partial class UpdateMangaDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Views_Comments_CommentId",
                table: "Views");

            migrationBuilder.DropIndex(
                name: "IX_Views_CommentId",
                table: "Views");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Views");

            migrationBuilder.AddColumn<float>(
                name: "Number",
                table: "Chapters",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "UploaderId",
                table: "Chapters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ApplicationUserManga",
                columns: table => new
                {
                    FollowedMangasId = table.Column<string>(type: "text", nullable: false),
                    FollowersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserManga", x => new { x.FollowedMangasId, x.FollowersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserManga_AspNetUsers_FollowersId",
                        column: x => x.FollowersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUserManga_Mangas_FollowedMangasId",
                        column: x => x.FollowedMangasId,
                        principalTable: "Mangas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    MangaId = table.Column<string>(type: "text", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rating_Mangas_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Mangas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_UploaderId",
                table: "Chapters",
                column: "UploaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserManga_FollowersId",
                table: "ApplicationUserManga",
                column: "FollowersId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_MangaId",
                table: "Rating",
                column: "MangaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_UserId",
                table: "Rating",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_AspNetUsers_UploaderId",
                table: "Chapters",
                column: "UploaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_AspNetUsers_UploaderId",
                table: "Chapters");

            migrationBuilder.DropTable(
                name: "ApplicationUserManga");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_UploaderId",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "UploaderId",
                table: "Chapters");

            migrationBuilder.AddColumn<string>(
                name: "CommentId",
                table: "Views",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Views_CommentId",
                table: "Views",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Views_Comments_CommentId",
                table: "Views",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
