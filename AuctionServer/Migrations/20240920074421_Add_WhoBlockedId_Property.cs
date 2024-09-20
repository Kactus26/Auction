using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionServer.Migrations
{
    /// <inheritdoc />
    public partial class Add_WhoBlockedId_Property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_WhoBlockedId",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_WhoBlockedId",
                table: "Friendships");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Friendships_WhoBlockedId",
                table: "Friendships",
                column: "WhoBlockedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_WhoBlockedId",
                table: "Friendships",
                column: "WhoBlockedId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
