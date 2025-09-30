using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMatrialIssueTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialIssuePermissions",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PermissionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(850)", maxLength: 850, nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    JobDepartmentId = table.Column<int>(type: "int", nullable: true),
                    DisbursementRequestId = table.Column<int>(type: "int", nullable: true),
                    DailyRestrictionId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialIssuePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_DailyRestrictions_DailyRestrictionId",
                        column: x => x.DailyRestrictionId,
                        principalSchema: "Accounting",
                        principalTable: "DailyRestrictions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_DisbursementRequests_DisbursementRequestId",
                        column: x => x.DisbursementRequestId,
                        principalSchema: "Inventory",
                        principalTable: "DisbursementRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_JobDepartments_JobDepartmentId",
                        column: x => x.JobDepartmentId,
                        principalSchema: "HR",
                        principalTable: "JobDepartments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "Inventory",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialIssueItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    MaterialIssuePermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialIssueItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialIssueItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialIssueItems_MaterialIssuePermissions_MaterialIssuePermissionId",
                        column: x => x.MaterialIssuePermissionId,
                        principalSchema: "Inventory",
                        principalTable: "MaterialIssuePermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssueItems_ItemId",
                table: "MaterialIssueItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssueItems_MaterialIssuePermissionId",
                table: "MaterialIssueItems",
                column: "MaterialIssuePermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_CreatedById",
                schema: "Inventory",
                table: "MaterialIssuePermissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_DailyRestrictionId",
                schema: "Inventory",
                table: "MaterialIssuePermissions",
                column: "DailyRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_DisbursementRequestId",
                schema: "Inventory",
                table: "MaterialIssuePermissions",
                column: "DisbursementRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_JobDepartmentId",
                schema: "Inventory",
                table: "MaterialIssuePermissions",
                column: "JobDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_StoreId",
                schema: "Inventory",
                table: "MaterialIssuePermissions",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_UpdatedById",
                schema: "Inventory",
                table: "MaterialIssuePermissions",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialIssueItems");

            migrationBuilder.DropTable(
                name: "MaterialIssuePermissions",
                schema: "Inventory");
        }
    }
}
