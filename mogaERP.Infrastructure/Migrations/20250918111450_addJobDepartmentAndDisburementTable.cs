using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addJobDepartmentAndDisburementTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "HR");

            migrationBuilder.CreateTable(
                name: "JobDepartments",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobDepartments_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobDepartments_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DisbursementRequests",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    JobDepartmentId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisbursementRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisbursementRequests_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DisbursementRequests_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DisbursementRequests_JobDepartments_JobDepartmentId",
                        column: x => x.JobDepartmentId,
                        principalSchema: "HR",
                        principalTable: "JobDepartments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DisbursementRequestItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DisbursementRequestId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisbursementRequestItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisbursementRequestItems_DisbursementRequests_DisbursementRequestId",
                        column: x => x.DisbursementRequestId,
                        principalSchema: "Inventory",
                        principalTable: "DisbursementRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DisbursementRequestItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementRequestItems_DisbursementRequestId",
                table: "DisbursementRequestItems",
                column: "DisbursementRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementRequestItems_ItemId",
                table: "DisbursementRequestItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementRequests_CreatedById",
                schema: "Inventory",
                table: "DisbursementRequests",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementRequests_JobDepartmentId",
                schema: "Inventory",
                table: "DisbursementRequests",
                column: "JobDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementRequests_UpdatedById",
                schema: "Inventory",
                table: "DisbursementRequests",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobDepartments_CreatedById",
                schema: "HR",
                table: "JobDepartments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobDepartments_UpdatedById",
                schema: "HR",
                table: "JobDepartments",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisbursementRequestItems");

            migrationBuilder.DropTable(
                name: "DisbursementRequests",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "JobDepartments",
                schema: "HR");
        }
    }
}
