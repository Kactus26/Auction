using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionServer.Migrations
{
    /// <inheritdoc />
    public partial class RemovePriceFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "StartPrice",
                table: "Lots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CurrentPrice",
                table: "Lots",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "StartPrice",
                table: "Lots",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
