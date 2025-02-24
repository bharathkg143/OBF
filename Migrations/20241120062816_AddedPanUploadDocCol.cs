using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderBookingFormApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedPanUploadDocCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PancardUploadDoc_FilePath",
                table: "confirm_salesbooking",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PancardUploadDoc_FilePath",
                table: "confirm_salesbooking");
        }
    }
}
