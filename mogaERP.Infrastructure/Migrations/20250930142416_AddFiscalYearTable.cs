using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFiscalYearTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FiscalYears",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FiscalYears_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FiscalYears_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FiscalYears_CreatedById",
                schema: "Accounting",
                table: "FiscalYears",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FiscalYears_UpdatedById",
                schema: "Accounting",
                table: "FiscalYears",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FiscalYears",
                schema: "Accounting");
        }
    }
}
