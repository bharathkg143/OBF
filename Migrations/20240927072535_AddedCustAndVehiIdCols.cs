using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderBookingFormApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedCustAndVehiIdCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CustomerID",
                table: "salesbooked_data",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "VehicleID",
                table: "salesbooked_data",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CustomerID",
                table: "confirm_salesbooking",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "VehicleID",
                table: "confirm_salesbooking",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "salesbooked_data");

            migrationBuilder.DropColumn(
                name: "VehicleID",
                table: "salesbooked_data");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "confirm_salesbooking");

            migrationBuilder.DropColumn(
                name: "VehicleID",
                table: "confirm_salesbooking");
        }
    }
}
