using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTreasryMovementAndOperations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StaffTreasuries",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    TreasuryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffTreasuries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffTreasuries_Staff_StaffId",
                        column: x => x.StaffId,
                        principalSchema: "HR",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffTreasuries_Treasuries_TreasuryId",
                        column: x => x.TreasuryId,
                        principalSchema: "Accounting",
                        principalTable: "Treasuries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TreasuryMovements",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreasuryNumber = table.Column<int>(type: "int", nullable: false),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpenedIn = table.Column<DateOnly>(type: "date", nullable: false),
                    ClosedIn = table.Column<DateOnly>(type: "date", nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    TotalCredits = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDebits = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TreasuryId = table.Column<int>(type: "int", nullable: false),
                    IsReEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasuryMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreasuryMovements_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TreasuryMovements_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TreasuryMovements_Treasuries_TreasuryId",
                        column: x => x.TreasuryId,
                        principalSchema: "Accounting",
                        principalTable: "Treasuries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TreasuryOperations",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: true),
                    ReceivedFrom = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    TreasuryId = table.Column<int>(type: "int", nullable: false),
                    TreasuryMovementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasuryOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreasuryOperations_Treasuries_TreasuryId",
                        column: x => x.TreasuryId,
                        principalSchema: "Accounting",
                        principalTable: "Treasuries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TreasuryOperations_TreasuryMovements_TreasuryMovementId",
                        column: x => x.TreasuryMovementId,
                        principalSchema: "Accounting",
                        principalTable: "TreasuryMovements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaffTreasuries_StaffId",
                schema: "Accounting",
                table: "StaffTreasuries",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffTreasuries_TreasuryId",
                schema: "Accounting",
                table: "StaffTreasuries",
                column: "TreasuryId");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryMovements_CreatedById",
                schema: "Accounting",
                table: "TreasuryMovements",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryMovements_TreasuryId",
                schema: "Accounting",
                table: "TreasuryMovements",
                column: "TreasuryId");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryMovements_UpdatedById",
                schema: "Accounting",
                table: "TreasuryMovements",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryOperations_TreasuryId",
                schema: "Accounting",
                table: "TreasuryOperations",
                column: "TreasuryId");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryOperations_TreasuryMovementId",
                schema: "Accounting",
                table: "TreasuryOperations",
                column: "TreasuryMovementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaffTreasuries",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "TreasuryOperations",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "TreasuryMovements",
                schema: "Accounting");
        }
    }
}
