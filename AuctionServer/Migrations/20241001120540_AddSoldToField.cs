using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionServer.Migrations
{
    /// <inheritdoc />
    public partial class AddSoldToField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoldToId",
                table: "Lots",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lots_SoldToId",
                table: "Lots",
                column: "SoldToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_Offers_SoldToId",
                table: "Lots",
                column: "SoldToId",
                principalTable: "Offers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lots_Offers_SoldToId",
                table: "Lots");

            migrationBuilder.DropIndex(
                name: "IX_Lots_SoldToId",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "SoldToId",
                table: "Lots");
        }
    }
}
