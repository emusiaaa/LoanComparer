using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankApp.Migrations
{
    /// <inheritdoc />
    public partial class Updatetablestructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClientJobEndDay",
                table: "NotRegisteredInquiries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ClientJobStartDay",
                table: "NotRegisteredInquiries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UserBirthDay",
                table: "NotRegisteredInquiries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ClientJobEndDay",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientJobStartDay",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserBirthDay",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientJobEndDay",
                table: "NotRegisteredInquiries");

            migrationBuilder.DropColumn(
                name: "ClientJobStartDay",
                table: "NotRegisteredInquiries");

            migrationBuilder.DropColumn(
                name: "UserBirthDay",
                table: "NotRegisteredInquiries");

            migrationBuilder.DropColumn(
                name: "ClientJobEndDay",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ClientJobStartDay",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserBirthDay",
                table: "AspNetUsers");
        }
    }
}
