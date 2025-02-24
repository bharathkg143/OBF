using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderBookingFormApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedApiRelatedCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "gatewayResponse",
                table: "confirm_salesbooking",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isEnquiryDataPost",
                table: "confirm_salesbooking",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gatewayResponse",
                table: "confirm_salesbooking");

            migrationBuilder.DropColumn(
                name: "isEnquiryDataPost",
                table: "confirm_salesbooking");
        }
    }
}
