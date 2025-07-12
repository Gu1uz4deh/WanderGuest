using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WanderQuest.Migrations
{
    /// <inheritdoc />
    public partial class TeamMembersRelationChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "TeamMemberImages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TeamMemberImages");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "TeamMemberImages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TeamMemberImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TeamMemberImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "TeamMemberImages",
                type: "datetime2",
                nullable: true);
        }
    }
}
