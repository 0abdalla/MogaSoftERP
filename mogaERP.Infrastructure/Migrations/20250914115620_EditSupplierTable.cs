using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditSupplierTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                schema: "Procurement",
                table: "PurchaseOrderItems");

            migrationBuilder.AddColumn<decimal>(
                name: "CreditLimit",
                schema: "Procurement",
                table: "Suppliers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentBalance",
                schema: "Procurement",
                table: "Suppliers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                schema: "Procurement",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditLimit",
                schema: "Procurement",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "CurrentBalance",
                schema: "Procurement",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                schema: "Procurement",
                table: "Suppliers");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                schema: "Procurement",
                table: "PurchaseOrderItems",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
