using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace OrderBookingFormApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedEnquiryAPITable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gatewayResponse",
                table: "confirm_salesbooking");

            migrationBuilder.DropColumn(
                name: "isEnquiryDataPost",
                table: "confirm_salesbooking");

            migrationBuilder.CreateTable(
                name: "enquiry_api_response",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    VehicleId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerName = table.Column<string>(type: "longtext", nullable: true),
                    ResponseMsg = table.Column<string>(type: "longtext", nullable: true),
                    IsEnquiryDataPosted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DateTimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enquiry_api_response", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enquiry_api_response");

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
    }
}
