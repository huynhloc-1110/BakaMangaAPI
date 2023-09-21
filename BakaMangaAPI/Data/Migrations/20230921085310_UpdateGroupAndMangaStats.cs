using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakaMangaAPI.Data.Migrations
{
    public partial class UpdateGroupAndMangaStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reacts_Comments_CommentId",
                table: "Reacts");

            migrationBuilder.DropIndex(
                name: "IX_Reacts_CommentId",
                table: "Reacts");

            migrationBuilder.DropIndex(
                name: "IX_Reacts_PostId",
                table: "Reacts");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "IsLeader",
                table: "GroupMembers");

            migrationBuilder.AddColumn<bool>(
                name: "IsMangaGroup",
                table: "Groups",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GroupRoles",
                table: "GroupMembers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reacts_CommentId_UserId",
                table: "Reacts",
                columns: new[] { "CommentId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reacts_PostId_UserId",
                table: "Reacts",
                columns: new[] { "PostId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId_MangaId",
                table: "Ratings",
                columns: new[] { "UserId", "MangaId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reacts_Comments_CommentId",
                table: "Reacts",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reacts_Comments_CommentId",
                table: "Reacts");

            migrationBuilder.DropIndex(
                name: "IX_Reacts_CommentId_UserId",
                table: "Reacts");

            migrationBuilder.DropIndex(
                name: "IX_Reacts_PostId_UserId",
                table: "Reacts");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserId_MangaId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "IsMangaGroup",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupRoles",
                table: "GroupMembers");

            migrationBuilder.AddColumn<bool>(
                name: "IsLeader",
                table: "GroupMembers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Reacts_CommentId",
                table: "Reacts",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reacts_PostId",
                table: "Reacts",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reacts_Comments_CommentId",
                table: "Reacts",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }
    }
}
