using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDebitNoticeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DebitNotices",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    CheckNumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    DailyRestrictionId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebitNotices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DebitNotices_AccountTrees_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Accounting",
                        principalTable: "AccountTrees",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DebitNotices_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DebitNotices_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DebitNotices_Banks_BankId",
                        column: x => x.BankId,
                        principalSchema: "Accounting",
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DebitNotices_DailyRestrictions_DailyRestrictionId",
                        column: x => x.DailyRestrictionId,
                        principalSchema: "Accounting",
                        principalTable: "DailyRestrictions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DebitNotices_AccountId",
                schema: "Accounting",
                table: "DebitNotices",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNotices_BankId",
                schema: "Accounting",
                table: "DebitNotices",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNotices_CreatedById",
                schema: "Accounting",
                table: "DebitNotices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNotices_DailyRestrictionId",
                schema: "Accounting",
                table: "DebitNotices",
                column: "DailyRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNotices_UpdatedById",
                schema: "Accounting",
                table: "DebitNotices",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DebitNotices",
                schema: "Accounting");
        }
    }
}
