using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryMgtSystem.Migrations
{
    /// <inheritdoc />
    public partial class _20260412 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "SalesOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "SalesOrders");
        }
    }
}
