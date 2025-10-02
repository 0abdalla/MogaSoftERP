using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerRelationshipToSalesQouatation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                schema: "Sales",
                table: "SalesQuotations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_SalesQuotations_CustomerId",
                schema: "Sales",
                table: "SalesQuotations",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesQuotations_Customers_CustomerId",
                schema: "Sales",
                table: "SalesQuotations",
                column: "CustomerId",
                principalSchema: "Sales",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesQuotations_Customers_CustomerId",
                schema: "Sales",
                table: "SalesQuotations");

            migrationBuilder.DropIndex(
                name: "IX_SalesQuotations_CustomerId",
                schema: "Sales",
                table: "SalesQuotations");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                schema: "Sales",
                table: "SalesQuotations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
