using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakaMangaAPI.Data.Migrations
{
    public partial class UpdateGroupMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "JoinedAt",
                table: "GroupMembers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_GroupId_UserId",
                table: "GroupMembers",
                columns: new[] { "GroupId", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_GroupId_UserId",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "JoinedAt",
                table: "GroupMembers");
        }
    }
}
