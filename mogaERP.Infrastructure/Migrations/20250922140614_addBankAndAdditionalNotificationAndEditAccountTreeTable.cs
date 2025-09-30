using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addBankAndAdditionalNotificationAndEditAccountTreeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyRestrictionDetails_AccountTrees_AccountId",
                schema: "Accounting",
                table: "DailyRestrictionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyRestrictionDetails_CostCenterTrees_CostCenterId",
                schema: "Accounting",
                table: "DailyRestrictionDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CostCenterTrees",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountTrees",
                schema: "Accounting",
                table: "AccountTrees");

            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.DropColumn(
                name: "AccountLevel",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.DropColumn(
                name: "AccountNature",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.DropColumn(
                name: "AccountTypeId",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.DropColumn(
                name: "AssetType",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.DropColumn(
                name: "DepreciationMethod",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.DropColumn(
                name: "FName",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.DropColumn(
                name: "IsDisToCostCenter",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.DropColumn(
                name: "PreCredit",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.DropColumn(
                name: "PreDebit",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.DropColumn(
                name: "CostCenterNumber",
                schema: "Accounting",
                table: "AccountTrees");

            migrationBuilder.RenameColumn(
                name: "ParentAccountId",
                schema: "Accounting",
                table: "CostCenterTrees",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "DepreciationYears",
                schema: "Accounting",
                table: "CostCenterTrees",
                newName: "IsExpences");

            migrationBuilder.RenameColumn(
                name: "DepreciationId",
                schema: "Accounting",
                table: "CostCenterTrees",
                newName: "DisplayOrder");

            migrationBuilder.RenameColumn(
                name: "AccumulatedDepreciationId",
                schema: "Accounting",
                table: "CostCenterTrees",
                newName: "CostLevel");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                schema: "Accounting",
                table: "AccountTrees",
                newName: "ParentAccountId");

            migrationBuilder.RenameColumn(
                name: "IsExpences",
                schema: "Accounting",
                table: "AccountTrees",
                newName: "DepreciationYears");

            migrationBuilder.RenameColumn(
                name: "DisplayOrder",
                schema: "Accounting",
                table: "AccountTrees",
                newName: "DepreciationId");

            migrationBuilder.RenameColumn(
                name: "CostLevel",
                schema: "Accounting",
                table: "AccountTrees",
                newName: "AccumulatedDepreciationId");

            migrationBuilder.AlterColumn<string>(
                name: "NameEN",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameAR",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "CostCenterId",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.AddColumn<int>(
                name: "CostCenterId",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "CostCenterNumber",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "NameEN",
                schema: "Accounting",
                table: "AccountTrees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NameAR",
                schema: "Accounting",
                table: "AccountTrees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Drop the old column
            migrationBuilder.DropColumn(
                name: "CostCenterId",
                schema: "Accounting",
                table: "AccountTrees");

            // Recreate it with desired definition
            migrationBuilder.AddColumn<int>(
                name: "CostCenterId",
                schema: "Accounting",
                table: "AccountTrees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                schema: "Accounting",
                table: "AccountTrees",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AccountLevel",
                schema: "Accounting",
                table: "AccountTrees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountNature",
                schema: "Accounting",
                table: "AccountTrees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                schema: "Accounting",
                table: "AccountTrees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountTypeId",
                schema: "Accounting",
                table: "AccountTrees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssetType",
                schema: "Accounting",
                table: "AccountTrees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepreciationMethod",
                schema: "Accounting",
                table: "AccountTrees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FName",
                schema: "Accounting",
                table: "AccountTrees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisToCostCenter",
                schema: "Accounting",
                table: "AccountTrees",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                schema: "Accounting",
                table: "AccountTrees",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PreCredit",
                schema: "Accounting",
                table: "AccountTrees",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PreDebit",
                schema: "Accounting",
                table: "AccountTrees",
                type: "float",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CostCenterTrees",
                schema: "Accounting",
                table: "CostCenterTrees",
                column: "CostCenterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountTrees",
                schema: "Accounting",
                table: "AccountTrees",
                column: "AccountId");

            migrationBuilder.CreateTable(
                name: "Banks",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InitialBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banks_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Banks_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdditionNotices",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    CheckNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_AdditionNotices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionNotices_AccountTrees_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Accounting",
                        principalTable: "AccountTrees",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdditionNotices_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdditionNotices_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdditionNotices_Banks_BankId",
                        column: x => x.BankId,
                        principalSchema: "Accounting",
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdditionNotices_DailyRestrictions_DailyRestrictionId",
                        column: x => x.DailyRestrictionId,
                        principalSchema: "Accounting",
                        principalTable: "DailyRestrictions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionNotices_AccountId",
                schema: "Accounting",
                table: "AdditionNotices",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionNotices_BankId",
                schema: "Accounting",
                table: "AdditionNotices",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionNotices_CreatedById",
                schema: "Accounting",
                table: "AdditionNotices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionNotices_DailyRestrictionId",
                schema: "Accounting",
                table: "AdditionNotices",
                column: "DailyRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionNotices_UpdatedById",
                schema: "Accounting",
                table: "AdditionNotices",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_CreatedById",
                schema: "Accounting",
                table: "Banks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_UpdatedById",
                schema: "Accounting",
                table: "Banks",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyRestrictionDetails_AccountTrees_AccountId",
                schema: "Accounting",
                table: "DailyRestrictionDetails",
                column: "AccountId",
                principalSchema: "Accounting",
                principalTable: "AccountTrees",
                principalColumn: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyRestrictionDetails_CostCenterTrees_CostCenterId",
                schema: "Accounting",
                table: "DailyRestrictionDetails",
                column: "CostCenterId",
                principalSchema: "Accounting",
                principalTable: "CostCenterTrees",
                principalColumn: "CostCenterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyRestrictionDetails_AccountTrees_AccountId",
                schema: "Accounting",
                table: "DailyRestrictionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyRestrictionDetails_CostCenterTrees_CostCenterId",
                schema: "Accounting",
                table: "DailyRestrictionDetails");

            migrationBuilder.DropTable(
                name: "AdditionNotices",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "Banks",
                schema: "Accounting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CostCenterTrees",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountTrees",
                schema: "Accounting",
                table: "AccountTrees");

            migrationBuilder.DropColumn(
                name: "CostCenterNumber",
                schema: "Accounting",
                table: "CostCenterTrees");

            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "Accounting",
                table: "AccountTrees");

            migrationBuilder.DropColumn(
                name: "AccountLevel",
                schema: "Accounting",
                table: "AccountTrees");

            migrationBuilder.DropColumn(
                name: "AccountNature",
                schema: "Accounting",
                table: "AccountTrees");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                schema: "Accounting",
                table: "AccountTrees");

            migrationBuilder.DropColumn(
                name: "AccountTypeId",
                schema: "Accounting",
                table: "AccountTrees");

            migrationBuilder.DropColumn(
                name: "AssetType",
                schema: "Accounting",
                table: "AccountTrees");

            migrationBuilder.DropColumn(
                name: "DepreciationMethod",
                schema: "Accounting",
                table: "AccountTrees");

            migrationBuilder.DropColumn(
                name: "FName",
                schema: "Accounting",
                table: "AccountTrees");

            migrationBuilder.DropColumn(
                name: "IsDisToCostCenter",
                schema: "Accounting",
                table: "AccountTrees");

            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                schema: "Accounting",
                table: "AccountTrees");

            migrationBuilder.DropColumn(
                name: "PreCredit",
                schema: "Accounting",
                table: "AccountTrees");

            migrationBuilder.DropColumn(
                name: "PreDebit",
                schema: "Accounting",
                table: "AccountTrees");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                schema: "Accounting",
                table: "CostCenterTrees",
                newName: "ParentAccountId");

            migrationBuilder.RenameColumn(
                name: "IsExpences",
                schema: "Accounting",
                table: "CostCenterTrees",
                newName: "DepreciationYears");

            migrationBuilder.RenameColumn(
                name: "DisplayOrder",
                schema: "Accounting",
                table: "CostCenterTrees",
                newName: "DepreciationId");

            migrationBuilder.RenameColumn(
                name: "CostLevel",
                schema: "Accounting",
                table: "CostCenterTrees",
                newName: "AccumulatedDepreciationId");

            migrationBuilder.RenameColumn(
                name: "ParentAccountId",
                schema: "Accounting",
                table: "AccountTrees",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "DepreciationYears",
                schema: "Accounting",
                table: "AccountTrees",
                newName: "IsExpences");

            migrationBuilder.RenameColumn(
                name: "DepreciationId",
                schema: "Accounting",
                table: "AccountTrees",
                newName: "DisplayOrder");

            migrationBuilder.RenameColumn(
                name: "AccumulatedDepreciationId",
                schema: "Accounting",
                table: "AccountTrees",
                newName: "CostLevel");

            migrationBuilder.AlterColumn<string>(
                name: "NameEN",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NameAR",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CostCenterId",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AccountLevel",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountNature",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountTypeId",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssetType",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepreciationMethod",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FName",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisToCostCenter",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PreCredit",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PreDebit",
                schema: "Accounting",
                table: "CostCenterTrees",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEN",
                schema: "Accounting",
                table: "AccountTrees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameAR",
                schema: "Accounting",
                table: "AccountTrees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CostCenterId",
                schema: "Accounting",
                table: "AccountTrees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "CostCenterNumber",
                schema: "Accounting",
                table: "AccountTrees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CostCenterTrees",
                schema: "Accounting",
                table: "CostCenterTrees",
                column: "AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountTrees",
                schema: "Accounting",
                table: "AccountTrees",
                column: "CostCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyRestrictionDetails_AccountTrees_AccountId",
                schema: "Accounting",
                table: "DailyRestrictionDetails",
                column: "AccountId",
                principalSchema: "Accounting",
                principalTable: "AccountTrees",
                principalColumn: "CostCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyRestrictionDetails_CostCenterTrees_CostCenterId",
                schema: "Accounting",
                table: "DailyRestrictionDetails",
                column: "CostCenterId",
                principalSchema: "Accounting",
                principalTable: "CostCenterTrees",
                principalColumn: "AccountId");
        }
    }
}
