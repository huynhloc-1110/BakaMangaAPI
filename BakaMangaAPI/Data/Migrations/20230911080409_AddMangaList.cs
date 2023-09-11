using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakaMangaAPI.Data.Migrations
{
    public partial class AddMangaList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MangaLists",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<string>(type: "text", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MangaLists_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MangaMangaList",
                columns: table => new
                {
                    MangaListsId = table.Column<string>(type: "text", nullable: false),
                    MangasId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaMangaList", x => new { x.MangaListsId, x.MangasId });
                    table.ForeignKey(
                        name: "FK_MangaMangaList_MangaLists_MangaListsId",
                        column: x => x.MangaListsId,
                        principalTable: "MangaLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MangaMangaList_Mangas_MangasId",
                        column: x => x.MangasId,
                        principalTable: "Mangas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MangaLists_OwnerId",
                table: "MangaLists",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaMangaList_MangasId",
                table: "MangaMangaList",
                column: "MangasId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MangaMangaList");

            migrationBuilder.DropTable(
                name: "MangaLists");
        }
    }
}
