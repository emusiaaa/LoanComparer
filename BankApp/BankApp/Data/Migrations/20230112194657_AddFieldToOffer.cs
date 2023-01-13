using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankApp.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldToOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApprovedByEmployee",
                table: "Offers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApprovedByEmployee",
                table: "Offers");
        }
    }
}
