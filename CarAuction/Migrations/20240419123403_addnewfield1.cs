using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarAuction.Migrations
{
    /// <inheritdoc />
    public partial class addnewfield1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FinalOwner",
                table: "Lots",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalOwner",
                table: "Lots");
        }
    }
}
