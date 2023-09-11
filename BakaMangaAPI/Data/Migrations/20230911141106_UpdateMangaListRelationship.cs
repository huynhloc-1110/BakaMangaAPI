using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakaMangaAPI.Data.Migrations
{
    public partial class UpdateMangaListRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MangaMangaList");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MangaLists",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "MangaListItems",
                columns: table => new
                {
                    MangaListId = table.Column<string>(type: "text", nullable: false),
                    MangaId = table.Column<string>(type: "text", nullable: false),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaListItems", x => new { x.MangaListId, x.MangaId });
                    table.ForeignKey(
                        name: "FK_MangaListItems_MangaLists_MangaListId",
                        column: x => x.MangaListId,
                        principalTable: "MangaLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaListItems_Mangas_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Mangas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MangaListItems_MangaId",
                table: "MangaListItems",
                column: "MangaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MangaListItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MangaLists");

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
                name: "IX_MangaMangaList_MangasId",
                table: "MangaMangaList",
                column: "MangasId");
        }
    }
}
