using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPaymentTerms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTerm_AspNetUsers_CreatedById",
                table: "PaymentTerm");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTerm_AspNetUsers_UpdatedById",
                table: "PaymentTerm");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTerm_SalesQuotations_SalesQuotationId",
                table: "PaymentTerm");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentTerm",
                table: "PaymentTerm");

            migrationBuilder.RenameTable(
                name: "PaymentTerm",
                newName: "PaymentTerms",
                newSchema: "Sales");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTerm_UpdatedById",
                schema: "Sales",
                table: "PaymentTerms",
                newName: "IX_PaymentTerms_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTerm_SalesQuotationId",
                schema: "Sales",
                table: "PaymentTerms",
                newName: "IX_PaymentTerms_SalesQuotationId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTerm_CreatedById",
                schema: "Sales",
                table: "PaymentTerms",
                newName: "IX_PaymentTerms_CreatedById");

            migrationBuilder.AlterColumn<string>(
                name: "Condition",
                schema: "Sales",
                table: "PaymentTerms",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentTerms",
                schema: "Sales",
                table: "PaymentTerms",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTerms_AspNetUsers_CreatedById",
                schema: "Sales",
                table: "PaymentTerms",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTerms_AspNetUsers_UpdatedById",
                schema: "Sales",
                table: "PaymentTerms",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTerms_SalesQuotations_SalesQuotationId",
                schema: "Sales",
                table: "PaymentTerms",
                column: "SalesQuotationId",
                principalSchema: "Sales",
                principalTable: "SalesQuotations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTerms_AspNetUsers_CreatedById",
                schema: "Sales",
                table: "PaymentTerms");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTerms_AspNetUsers_UpdatedById",
                schema: "Sales",
                table: "PaymentTerms");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTerms_SalesQuotations_SalesQuotationId",
                schema: "Sales",
                table: "PaymentTerms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentTerms",
                schema: "Sales",
                table: "PaymentTerms");

            migrationBuilder.RenameTable(
                name: "PaymentTerms",
                schema: "Sales",
                newName: "PaymentTerm");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTerms_UpdatedById",
                table: "PaymentTerm",
                newName: "IX_PaymentTerm_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTerms_SalesQuotationId",
                table: "PaymentTerm",
                newName: "IX_PaymentTerm_SalesQuotationId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTerms_CreatedById",
                table: "PaymentTerm",
                newName: "IX_PaymentTerm_CreatedById");

            migrationBuilder.AlterColumn<string>(
                name: "Condition",
                table: "PaymentTerm",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentTerm",
                table: "PaymentTerm",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTerm_AspNetUsers_CreatedById",
                table: "PaymentTerm",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTerm_AspNetUsers_UpdatedById",
                table: "PaymentTerm",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTerm_SalesQuotations_SalesQuotationId",
                table: "PaymentTerm",
                column: "SalesQuotationId",
                principalSchema: "Sales",
                principalTable: "SalesQuotations",
                principalColumn: "Id");
        }
    }
}
