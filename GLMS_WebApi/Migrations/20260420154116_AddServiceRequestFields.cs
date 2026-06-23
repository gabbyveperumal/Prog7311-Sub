using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GLMS_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceRequestFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CostUSD",
                table: "ServiceRequests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ServiceRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostUSD",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ServiceRequests");
        }
    }
}
