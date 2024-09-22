using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionServer.Migrations
{
    /// <inheritdoc />
    public partial class AddFriendsRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Relations",
                table: "Friendships",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Relations",
                table: "Friendships");
        }
    }
}
