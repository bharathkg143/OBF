using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace OrderBookingFormApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedTableSMSTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sms_templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SMSTemplateId = table.Column<string>(type: "longtext", nullable: true),
                    SMSTemplateName = table.Column<string>(type: "longtext", nullable: true),
                    SMSApi = table.Column<string>(type: "longtext", nullable: true),
                    SMSTemplate = table.Column<string>(type: "longtext", nullable: true),
                    SMSSenderHeaderId = table.Column<string>(type: "longtext", nullable: true),
                    SMSRoute = table.Column<string>(type: "longtext", nullable: true),
                    SMSType = table.Column<string>(type: "longtext", nullable: true),
                    DealerName = table.Column<string>(type: "longtext", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sms_templates", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sms_templates");
        }
    }
}
