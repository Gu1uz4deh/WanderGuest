using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WanderQuest.Migrations
{
    /// <inheritdoc />
    public partial class TeamMembersRelationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeamMemberImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamMemberId = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMemberImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMemberImages_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamMemberImages_TeamMembers_TeamMemberId",
                        column: x => x.TeamMemberId,
                        principalTable: "TeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamMemberImages_ImageId",
                table: "TeamMemberImages",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMemberImages_TeamMemberId",
                table: "TeamMemberImages",
                column: "TeamMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamMemberImages");
        }
    }
}
