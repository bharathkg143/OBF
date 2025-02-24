using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderBookingFormApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedAdharPanRelatedCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AadharBackImg_FilePath",
                table: "confirm_salesbooking",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AadharFrontImg_FilePath",
                table: "confirm_salesbooking",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AadharUploadDoc_FilePath",
                table: "confirm_salesbooking",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PanCardImg_FilePath",
                table: "confirm_salesbooking",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AadharBackImg_FilePath",
                table: "confirm_salesbooking");

            migrationBuilder.DropColumn(
                name: "AadharFrontImg_FilePath",
                table: "confirm_salesbooking");

            migrationBuilder.DropColumn(
                name: "AadharUploadDoc_FilePath",
                table: "confirm_salesbooking");

            migrationBuilder.DropColumn(
                name: "PanCardImg_FilePath",
                table: "confirm_salesbooking");
        }
    }
}
