using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class salesQoutationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Sales");

            migrationBuilder.CreateTable(
                name: "SalesQuotations",
                schema: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ValidUntil = table.Column<DateOnly>(type: "date", nullable: false),
                    IsTaxIncluded = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesQuotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesQuotations_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesQuotations_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                schema: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    QuotationId = table.Column<int>(type: "int", nullable: false),
                    IsTaxIncluded = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_SalesQuotations_QuotationId",
                        column: x => x.QuotationId,
                        principalSchema: "Sales",
                        principalTable: "SalesQuotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTerm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Condition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalesQuotationId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTerm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentTerm_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentTerm_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PaymentTerm_SalesQuotations_SalesQuotationId",
                        column: x => x.SalesQuotationId,
                        principalSchema: "Sales",
                        principalTable: "SalesQuotations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuotationItems",
                schema: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotationId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesQuotationId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationItems_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuotationItems_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuotationItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuotationItems_SalesQuotations_SalesQuotationId",
                        column: x => x.SalesQuotationId,
                        principalSchema: "Sales",
                        principalTable: "SalesQuotations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                schema: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "Sales",
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_CreatedById",
                schema: "Sales",
                table: "InvoiceItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                schema: "Sales",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_ItemId",
                schema: "Sales",
                table: "InvoiceItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_UpdatedById",
                schema: "Sales",
                table: "InvoiceItems",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CreatedById",
                schema: "Sales",
                table: "Invoices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_QuotationId",
                schema: "Sales",
                table: "Invoices",
                column: "QuotationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_UpdatedById",
                schema: "Sales",
                table: "Invoices",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTerm_CreatedById",
                table: "PaymentTerm",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTerm_SalesQuotationId",
                table: "PaymentTerm",
                column: "SalesQuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTerm_UpdatedById",
                table: "PaymentTerm",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationItems_CreatedById",
                schema: "Sales",
                table: "QuotationItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationItems_ItemId",
                schema: "Sales",
                table: "QuotationItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationItems_SalesQuotationId",
                schema: "Sales",
                table: "QuotationItems",
                column: "SalesQuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationItems_UpdatedById",
                schema: "Sales",
                table: "QuotationItems",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SalesQuotations_CreatedById",
                schema: "Sales",
                table: "SalesQuotations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SalesQuotations_UpdatedById",
                schema: "Sales",
                table: "SalesQuotations",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceItems",
                schema: "Sales");

            migrationBuilder.DropTable(
                name: "PaymentTerm");

            migrationBuilder.DropTable(
                name: "QuotationItems",
                schema: "Sales");

            migrationBuilder.DropTable(
                name: "Invoices",
                schema: "Sales");

            migrationBuilder.DropTable(
                name: "SalesQuotations",
                schema: "Sales");
        }
    }
}
