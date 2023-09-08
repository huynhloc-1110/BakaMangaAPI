using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakaMangaAPI.Data.Migrations
{
    public partial class UpdateApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserApplicationUser_AspNetUsers_FolloweesId",
                table: "ApplicationUserApplicationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserApplicationUser",
                table: "ApplicationUserApplicationUser");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserApplicationUser_FollowersId",
                table: "ApplicationUserApplicationUser");

            migrationBuilder.RenameColumn(
                name: "FolloweesId",
                table: "ApplicationUserApplicationUser",
                newName: "FollowingsId");

            migrationBuilder.AddColumn<string>(
                name: "Biography",
                table: "AspNetUsers",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserApplicationUser",
                table: "ApplicationUserApplicationUser",
                columns: new[] { "FollowersId", "FollowingsId" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserApplicationUser_FollowingsId",
                table: "ApplicationUserApplicationUser",
                column: "FollowingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserApplicationUser_AspNetUsers_FollowingsId",
                table: "ApplicationUserApplicationUser",
                column: "FollowingsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserApplicationUser_AspNetUsers_FollowingsId",
                table: "ApplicationUserApplicationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserApplicationUser",
                table: "ApplicationUserApplicationUser");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserApplicationUser_FollowingsId",
                table: "ApplicationUserApplicationUser");

            migrationBuilder.DropColumn(
                name: "Biography",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "FollowingsId",
                table: "ApplicationUserApplicationUser",
                newName: "FolloweesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserApplicationUser",
                table: "ApplicationUserApplicationUser",
                columns: new[] { "FolloweesId", "FollowersId" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserApplicationUser_FollowersId",
                table: "ApplicationUserApplicationUser",
                column: "FollowersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserApplicationUser_AspNetUsers_FolloweesId",
                table: "ApplicationUserApplicationUser",
                column: "FolloweesId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
