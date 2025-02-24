using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace OrderBookingFormApp.Migrations
{
    /// <inheritdoc />
    public partial class DbCreationForOBF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "confirm_salesbooking",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CustomerName = table.Column<string>(type: "longtext", nullable: true),
                    MobileNo = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumbers = table.Column<string>(type: "longtext", nullable: true),
                    OtherContactNo = table.Column<string>(type: "longtext", nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: true),
                    PanCardNo = table.Column<string>(type: "longtext", nullable: true),
                    FatherOrHusbandName = table.Column<string>(type: "longtext", nullable: true),
                    DOB = table.Column<string>(type: "longtext", nullable: true),
                    DOA = table.Column<string>(type: "longtext", nullable: true),
                    Age = table.Column<string>(type: "longtext", nullable: true),
                    Address = table.Column<string>(type: "longtext", nullable: true),
                    PinCode = table.Column<string>(type: "longtext", nullable: true),
                    Village = table.Column<string>(type: "longtext", nullable: true),
                    City = table.Column<string>(type: "longtext", nullable: true),
                    Taluk = table.Column<string>(type: "longtext", nullable: true),
                    District = table.Column<string>(type: "longtext", nullable: true),
                    State = table.Column<string>(type: "longtext", nullable: true),
                    DmsObfNo = table.Column<string>(type: "longtext", nullable: true),
                    BookingDate = table.Column<string>(type: "longtext", nullable: true),
                    Model = table.Column<string>(type: "longtext", nullable: true),
                    Variant = table.Column<string>(type: "longtext", nullable: true),
                    Color = table.Column<string>(type: "longtext", nullable: true),
                    TentativeWaitPeriod = table.Column<string>(type: "longtext", nullable: true),
                    ExShowroomPrice = table.Column<string>(type: "longtext", nullable: true),
                    RegistrationCharges = table.Column<string>(type: "longtext", nullable: true),
                    InsurancePrice = table.Column<string>(type: "longtext", nullable: true),
                    TempRegCharges = table.Column<string>(type: "longtext", nullable: true),
                    EwOptional = table.Column<string>(type: "longtext", nullable: true),
                    Accessories = table.Column<string>(type: "longtext", nullable: true),
                    OthersIfAny = table.Column<string>(type: "longtext", nullable: true),
                    OthersDescribe = table.Column<string>(type: "longtext", nullable: true),
                    Discount = table.Column<string>(type: "longtext", nullable: true),
                    CGST14Percent = table.Column<string>(type: "longtext", nullable: true),
                    SGST14Percent = table.Column<string>(type: "longtext", nullable: true),
                    CESS1Percent = table.Column<string>(type: "longtext", nullable: true),
                    OnRoadPrice = table.Column<string>(type: "longtext", nullable: true),
                    Amount = table.Column<string>(type: "longtext", nullable: true),
                    PaymentMode = table.Column<string>(type: "longtext", nullable: true),
                    PaymentReference = table.Column<string>(type: "longtext", nullable: true),
                    Exchangestatus = table.Column<string>(type: "longtext", nullable: true),
                    MSILListedCorporate = table.Column<string>(type: "longtext", nullable: true),
                    CorporateName = table.Column<string>(type: "longtext", nullable: true),
                    Finance = table.Column<string>(type: "longtext", nullable: true),
                    ExistingCarModel = table.Column<string>(type: "longtext", nullable: true),
                    RegistrationNo = table.Column<string>(type: "longtext", nullable: true),
                    Expecteddate = table.Column<string>(type: "longtext", nullable: true),
                    Promiseddate = table.Column<string>(type: "longtext", nullable: true),
                    CustomerType = table.Column<string>(type: "longtext", nullable: true),
                    Profession = table.Column<string>(type: "longtext", nullable: true),
                    SourceOfEnquiry = table.Column<string>(type: "longtext", nullable: true),
                    SourceDetails = table.Column<string>(type: "longtext", nullable: true),
                    CustomerSignature_FilePath = table.Column<string>(type: "longtext", nullable: true),
                    CustomerPhoto_FilePath = table.Column<string>(type: "longtext", nullable: true),
                    ObfPDF_FilePath = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_confirm_salesbooking", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "generated_otp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    OTP = table.Column<int>(type: "int", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_generated_otp", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "salesbooked_data",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CustomerName = table.Column<string>(type: "longtext", nullable: true),
                    MobileNo = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumbers = table.Column<string>(type: "longtext", nullable: true),
                    OtherContactNo = table.Column<string>(type: "longtext", nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: true),
                    PanCardNo = table.Column<string>(type: "longtext", nullable: true),
                    FatherOrHusbandName = table.Column<string>(type: "longtext", nullable: true),
                    DOB = table.Column<string>(type: "longtext", nullable: true),
                    DOA = table.Column<string>(type: "longtext", nullable: true),
                    Age = table.Column<string>(type: "longtext", nullable: true),
                    Address = table.Column<string>(type: "longtext", nullable: true),
                    PinCode = table.Column<string>(type: "longtext", nullable: true),
                    Village = table.Column<string>(type: "longtext", nullable: true),
                    City = table.Column<string>(type: "longtext", nullable: true),
                    Taluk = table.Column<string>(type: "longtext", nullable: true),
                    District = table.Column<string>(type: "longtext", nullable: true),
                    State = table.Column<string>(type: "longtext", nullable: true),
                    DmsObfNo = table.Column<string>(type: "longtext", nullable: true),
                    BookingDate = table.Column<string>(type: "longtext", nullable: true),
                    Model = table.Column<string>(type: "longtext", nullable: true),
                    Variant = table.Column<string>(type: "longtext", nullable: true),
                    Color = table.Column<string>(type: "longtext", nullable: true),
                    TentativeWaitPeriod = table.Column<string>(type: "longtext", nullable: true),
                    ExShowroomPrice = table.Column<string>(type: "longtext", nullable: true),
                    RegistrationCharges = table.Column<string>(type: "longtext", nullable: true),
                    InsurancePrice = table.Column<string>(type: "longtext", nullable: true),
                    TempRegCharges = table.Column<string>(type: "longtext", nullable: true),
                    EwOptional = table.Column<string>(type: "longtext", nullable: true),
                    Accessories = table.Column<string>(type: "longtext", nullable: true),
                    OthersIfAny = table.Column<string>(type: "longtext", nullable: true),
                    OthersDescribe = table.Column<string>(type: "longtext", nullable: true),
                    Discount = table.Column<string>(type: "longtext", nullable: true),
                    CGST14Percent = table.Column<string>(type: "longtext", nullable: true),
                    SGST14Percent = table.Column<string>(type: "longtext", nullable: true),
                    CESS1Percent = table.Column<string>(type: "longtext", nullable: true),
                    OnRoadPrice = table.Column<string>(type: "longtext", nullable: true),
                    Amount = table.Column<string>(type: "longtext", nullable: true),
                    PaymentMode = table.Column<string>(type: "longtext", nullable: true),
                    PaymentReference = table.Column<string>(type: "longtext", nullable: true),
                    Exchangestatus = table.Column<string>(type: "longtext", nullable: true),
                    MSILListedCorporate = table.Column<string>(type: "longtext", nullable: true),
                    CorporateName = table.Column<string>(type: "longtext", nullable: true),
                    Finance = table.Column<string>(type: "longtext", nullable: true),
                    ExistingCarModel = table.Column<string>(type: "longtext", nullable: true),
                    RegistrationNo = table.Column<string>(type: "longtext", nullable: true),
                    Expecteddate = table.Column<string>(type: "longtext", nullable: true),
                    Promiseddate = table.Column<string>(type: "longtext", nullable: true),
                    CustomerType = table.Column<string>(type: "longtext", nullable: true),
                    Profession = table.Column<string>(type: "longtext", nullable: true),
                    SourceOfEnquiry = table.Column<string>(type: "longtext", nullable: true),
                    SourceDetails = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salesbooked_data", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sms_interaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    MobileNumber = table.Column<long>(type: "bigint", nullable: true),
                    Message = table.Column<string>(type: "varchar(1500)", maxLength: 1500, nullable: true),
                    SmsType = table.Column<string>(type: "longtext", nullable: true),
                    MsgStatus = table.Column<string>(type: "longtext", nullable: true),
                    ResponseFromGateway = table.Column<string>(type: "longtext", nullable: true),
                    InteractionDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sms_interaction", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "confirm_salesbooking");

            migrationBuilder.DropTable(
                name: "generated_otp");

            migrationBuilder.DropTable(
                name: "salesbooked_data");

            migrationBuilder.DropTable(
                name: "sms_interaction");
        }
    }
}
