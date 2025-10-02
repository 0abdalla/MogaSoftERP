using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesInvoicesAndTaxes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_SalesQuotations_QuotationId",
                schema: "Sales",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_QuotationId",
                schema: "Sales",
                table: "Invoices");

            migrationBuilder.AlterColumn<int>(
                name: "QuotationId",
                schema: "Sales",
                table: "Invoices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                schema: "Sales",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RevenueTypeId",
                schema: "Sales",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaxId",
                schema: "Sales",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Taxes_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Taxes_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerId",
                schema: "Sales",
                table: "Invoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_QuotationId",
                schema: "Sales",
                table: "Invoices",
                column: "QuotationId",
                unique: true,
                filter: "[QuotationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_RevenueTypeId",
                schema: "Sales",
                table: "Invoices",
                column: "RevenueTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_TaxId",
                schema: "Sales",
                table: "Invoices",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_CreatedById",
                table: "Taxes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_UpdatedById",
                table: "Taxes",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_AccountTrees_RevenueTypeId",
                schema: "Sales",
                table: "Invoices",
                column: "RevenueTypeId",
                principalSchema: "Accounting",
                principalTable: "AccountTrees",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                schema: "Sales",
                table: "Invoices",
                column: "CustomerId",
                principalSchema: "Sales",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_SalesQuotations_QuotationId",
                schema: "Sales",
                table: "Invoices",
                column: "QuotationId",
                principalSchema: "Sales",
                principalTable: "SalesQuotations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Taxes_TaxId",
                schema: "Sales",
                table: "Invoices",
                column: "TaxId",
                principalTable: "Taxes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_AccountTrees_RevenueTypeId",
                schema: "Sales",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                schema: "Sales",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_SalesQuotations_QuotationId",
                schema: "Sales",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Taxes_TaxId",
                schema: "Sales",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_CustomerId",
                schema: "Sales",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_QuotationId",
                schema: "Sales",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_RevenueTypeId",
                schema: "Sales",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_TaxId",
                schema: "Sales",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "Sales",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RevenueTypeId",
                schema: "Sales",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TaxId",
                schema: "Sales",
                table: "Invoices");

            migrationBuilder.AlterColumn<int>(
                name: "QuotationId",
                schema: "Sales",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_QuotationId",
                schema: "Sales",
                table: "Invoices",
                column: "QuotationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_SalesQuotations_QuotationId",
                schema: "Sales",
                table: "Invoices",
                column: "QuotationId",
                principalSchema: "Sales",
                principalTable: "SalesQuotations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
