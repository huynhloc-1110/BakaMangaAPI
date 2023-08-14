using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakaMangaAPI.Data.Migrations
{
    public partial class UpdateGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UploadingGroupId",
                table: "Chapters",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Biography = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupMember",
                columns: table => new
                {
                    GroupId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    IsLeader = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMember", x => new { x.GroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_GroupMember_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupMember_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_UploadingGroupId",
                table: "Chapters",
                column: "UploadingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_UserId",
                table: "GroupMember",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Group_UploadingGroupId",
                table: "Chapters",
                column: "UploadingGroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Group_UploadingGroupId",
                table: "Chapters");

            migrationBuilder.DropTable(
                name: "GroupMember");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_UploadingGroupId",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "UploadingGroupId",
                table: "Chapters");
        }
    }
}
