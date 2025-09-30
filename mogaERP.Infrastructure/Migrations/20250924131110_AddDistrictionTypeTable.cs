using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDistrictionTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RestrictionTypes",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestrictionTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestrictionTypes_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RestrictionTypes_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyRestrictions_RestrictionTypeId",
                schema: "Accounting",
                table: "DailyRestrictions",
                column: "RestrictionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RestrictionTypes_CreatedById",
                schema: "Accounting",
                table: "RestrictionTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RestrictionTypes_UpdatedById",
                schema: "Accounting",
                table: "RestrictionTypes",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyRestrictions_RestrictionTypes_RestrictionTypeId",
                schema: "Accounting",
                table: "DailyRestrictions",
                column: "RestrictionTypeId",
                principalSchema: "Accounting",
                principalTable: "RestrictionTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyRestrictions_RestrictionTypes_RestrictionTypeId",
                schema: "Accounting",
                table: "DailyRestrictions");

            migrationBuilder.DropTable(
                name: "RestrictionTypes",
                schema: "Accounting");

            migrationBuilder.DropIndex(
                name: "IX_DailyRestrictions_RestrictionTypeId",
                schema: "Accounting",
                table: "DailyRestrictions");
        }
    }
}
