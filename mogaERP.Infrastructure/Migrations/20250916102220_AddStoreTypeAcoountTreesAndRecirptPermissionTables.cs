using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreTypeAcoountTreesAndRecirptPermissionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Accounting");

            migrationBuilder.AddColumn<int>(
                name: "StoreTypeId",
                schema: "Inventory",
                table: "Stores",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccountingGuidances",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingGuidances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountingGuidances_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountingGuidances_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccountTrees",
                schema: "Accounting",
                columns: table => new
                {
                    CostCenterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CostCenterNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    CostLevel = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: true),
                    IsParent = table.Column<bool>(type: "bit", nullable: true),
                    IsPost = table.Column<bool>(type: "bit", nullable: true),
                    IsExpences = table.Column<int>(type: "int", nullable: true),
                    IsGroup = table.Column<bool>(type: "bit", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTrees", x => x.CostCenterId);
                });

            migrationBuilder.CreateTable(
                name: "CostCenterTrees",
                schema: "Accounting",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentAccountId = table.Column<int>(type: "int", nullable: true),
                    AccountLevel = table.Column<int>(type: "int", nullable: true),
                    AccountTypeId = table.Column<int>(type: "int", nullable: true),
                    NameAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsParent = table.Column<bool>(type: "bit", nullable: true),
                    IsGroup = table.Column<bool>(type: "bit", nullable: true),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: true),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    AccountNature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: true),
                    IsDisToCostCenter = table.Column<bool>(type: "bit", nullable: true),
                    IsPost = table.Column<bool>(type: "bit", nullable: true),
                    AssetType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepreciationMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepreciationYears = table.Column<int>(type: "int", nullable: true),
                    DepreciationId = table.Column<int>(type: "int", nullable: true),
                    AccumulatedDepreciationId = table.Column<int>(type: "int", nullable: true),
                    PreCredit = table.Column<double>(type: "float", nullable: true),
                    PreDebit = table.Column<double>(type: "float", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCenterTrees", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "StoreTypes",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreTypes_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreTypes_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DailyRestrictions",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestrictionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RestrictionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RestrictionTypeId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DocumentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AccountingGuidanceId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRestrictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyRestrictions_AccountingGuidances_AccountingGuidanceId",
                        column: x => x.AccountingGuidanceId,
                        principalSchema: "Accounting",
                        principalTable: "AccountingGuidances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DailyRestrictions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DailyRestrictions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DailyRestrictionDetails",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyRestrictionId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: true),
                    From = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    To = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRestrictionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyRestrictionDetails_AccountTrees_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Accounting",
                        principalTable: "AccountTrees",
                        principalColumn: "CostCenterId");
                    table.ForeignKey(
                        name: "FK_DailyRestrictionDetails_CostCenterTrees_CostCenterId",
                        column: x => x.CostCenterId,
                        principalSchema: "Accounting",
                        principalTable: "CostCenterTrees",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_DailyRestrictionDetails_DailyRestrictions_DailyRestrictionId",
                        column: x => x.DailyRestrictionId,
                        principalSchema: "Accounting",
                        principalTable: "DailyRestrictions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptPermissions",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PermissionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: true),
                    DailyRestrictionId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_DailyRestrictions_DailyRestrictionId",
                        column: x => x.DailyRestrictionId,
                        principalSchema: "Accounting",
                        principalTable: "DailyRestrictions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "Procurement",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "Inventory",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "Procurement",
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptPermissionItems",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ReceiptPermissionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptPermissionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissionItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissionItems_ReceiptPermissions_ReceiptPermissionId",
                        column: x => x.ReceiptPermissionId,
                        principalSchema: "Inventory",
                        principalTable: "ReceiptPermissions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_StoreTypeId",
                schema: "Inventory",
                table: "Stores",
                column: "StoreTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingGuidances_CreatedById",
                schema: "Accounting",
                table: "AccountingGuidances",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingGuidances_UpdatedById",
                schema: "Accounting",
                table: "AccountingGuidances",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRestrictionDetails_AccountId",
                schema: "Accounting",
                table: "DailyRestrictionDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRestrictionDetails_CostCenterId",
                schema: "Accounting",
                table: "DailyRestrictionDetails",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRestrictionDetails_DailyRestrictionId",
                schema: "Accounting",
                table: "DailyRestrictionDetails",
                column: "DailyRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRestrictions_AccountingGuidanceId",
                schema: "Accounting",
                table: "DailyRestrictions",
                column: "AccountingGuidanceId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRestrictions_CreatedById",
                schema: "Accounting",
                table: "DailyRestrictions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRestrictions_UpdatedById",
                schema: "Accounting",
                table: "DailyRestrictions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissionItems_ItemId",
                schema: "Inventory",
                table: "ReceiptPermissionItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissionItems_ReceiptPermissionId",
                schema: "Inventory",
                table: "ReceiptPermissionItems",
                column: "ReceiptPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_CreatedById",
                schema: "Inventory",
                table: "ReceiptPermissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_DailyRestrictionId",
                schema: "Inventory",
                table: "ReceiptPermissions",
                column: "DailyRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_PurchaseOrderId",
                schema: "Inventory",
                table: "ReceiptPermissions",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_StoreId",
                schema: "Inventory",
                table: "ReceiptPermissions",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_SupplierId",
                schema: "Inventory",
                table: "ReceiptPermissions",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_UpdatedById",
                schema: "Inventory",
                table: "ReceiptPermissions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTypes_CreatedById",
                schema: "Inventory",
                table: "StoreTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTypes_UpdatedById",
                schema: "Inventory",
                table: "StoreTypes",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_StoreTypes_StoreTypeId",
                schema: "Inventory",
                table: "Stores",
                column: "StoreTypeId",
                principalSchema: "Inventory",
                principalTable: "StoreTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_StoreTypes_StoreTypeId",
                schema: "Inventory",
                table: "Stores");

            migrationBuilder.DropTable(
                name: "DailyRestrictionDetails",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "ReceiptPermissionItems",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "StoreTypes",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "AccountTrees",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "CostCenterTrees",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "ReceiptPermissions",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "DailyRestrictions",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "AccountingGuidances",
                schema: "Accounting");

            migrationBuilder.DropIndex(
                name: "IX_Stores_StoreTypeId",
                schema: "Inventory",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "StoreTypeId",
                schema: "Inventory",
                table: "Stores");
        }
    }
}
