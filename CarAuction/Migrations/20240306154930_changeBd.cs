using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarAuction.Migrations
{
    /// <inheritdoc />
    public partial class changeBd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_AspNetUsers_ApplicationUserId",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Auctions_AuctionId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_AuctionId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "AuctionId",
                table: "Bids");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Bids",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Bids_ApplicationUserId",
                table: "Bids",
                newName: "IX_Bids_UserId1");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Bids",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AuctionDate",
                table: "Bids",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotId",
                table: "Bids",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LotId1",
                table: "Bids",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Auctions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Lots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    AuctionId = table.Column<int>(type: "int", nullable: false),
                    FinalCost = table.Column<int>(type: "int", nullable: false),
                    isSaled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lots_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lots_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bids_LotId1",
                table: "Bids",
                column: "LotId1");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_AuctionId",
                table: "Lots",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_VehicleId",
                table: "Lots",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_AspNetUsers_UserId1",
                table: "Bids",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Lots_LotId1",
                table: "Bids",
                column: "LotId1",
                principalTable: "Lots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_AspNetUsers_UserId1",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Lots_LotId1",
                table: "Bids");

            migrationBuilder.DropTable(
                name: "Lots");

            migrationBuilder.DropIndex(
                name: "IX_Bids_LotId1",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "AuctionDate",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "LotId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "LotId1",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Auctions");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Bids",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Bids_UserId1",
                table: "Bids",
                newName: "IX_Bids_ApplicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Bids",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AuctionId",
                table: "Bids",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bids_AuctionId",
                table: "Bids",
                column: "AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_AspNetUsers_ApplicationUserId",
                table: "Bids",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Auctions_AuctionId",
                table: "Bids",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
