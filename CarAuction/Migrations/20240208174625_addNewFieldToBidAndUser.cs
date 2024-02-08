using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarAuction.Migrations
{
    /// <inheritdoc />
    public partial class addNewFieldToBidAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Bids",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Bids",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_ApplicationUserId",
                table: "Bids",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_AspNetUsers_ApplicationUserId",
                table: "Bids",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_AspNetUsers_ApplicationUserId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_ApplicationUserId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bids");
        }
    }
}
