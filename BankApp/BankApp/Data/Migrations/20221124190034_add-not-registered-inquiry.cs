using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankApp.Migrations
{
    /// <inheritdoc />
    public partial class addnotregisteredinquiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotRegisteredInquiries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstallmentsCount = table.Column<int>(type: "int", nullable: false),
                    LoanValue = table.Column<float>(type: "real", nullable: false),
                    UserFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientGovernmentIDNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientGovernmentIDType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientJobType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientIncomeLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotRegisteredInquiries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotRegisteredInquiries");
        }
    }
}
