using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarAuction.Migrations
{
    /// <inheritdoc />
    public partial class deletefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Auctions_AuctionId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_AuctionId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "AuctionId",
                table: "Vehicles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuctionId",
                table: "Vehicles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_AuctionId",
                table: "Vehicles",
                column: "AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Auctions_AuctionId",
                table: "Vehicles",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id");
        }
    }
}
