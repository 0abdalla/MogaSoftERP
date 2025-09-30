using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdvacneTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAdvances_AspNetUsers_CreatedById",
                schema: "HR",
                table: "EmployeeAdvances");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAdvances_AspNetUsers_UpdatedById",
                schema: "HR",
                table: "EmployeeAdvances");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAdvances_CreatedById",
                schema: "HR",
                table: "EmployeeAdvances");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAdvances_UpdatedById",
                schema: "HR",
                table: "EmployeeAdvances");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                schema: "HR",
                table: "EmployeeAdvances");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "HR",
                table: "EmployeeAdvances");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "EmployeeAdvances");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                schema: "HR",
                table: "EmployeeAdvances");

            migrationBuilder.RenameColumn(
                name: "UpdatedOn",
                schema: "HR",
                table: "EmployeeAdvances",
                newName: "ModifiedDate");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "HR",
                table: "EmployeeAdvances",
                newName: "StaffAdvanceId");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "HR",
                table: "EmployeeAdvances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "HR",
                table: "EmployeeAdvances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                schema: "HR",
                table: "EmployeeAdvances",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "HR",
                table: "EmployeeAdvances");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "HR",
                table: "EmployeeAdvances");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "HR",
                table: "EmployeeAdvances");

            migrationBuilder.RenameColumn(
                name: "ModifiedDate",
                schema: "HR",
                table: "EmployeeAdvances",
                newName: "UpdatedOn");

            migrationBuilder.RenameColumn(
                name: "StaffAdvanceId",
                schema: "HR",
                table: "EmployeeAdvances",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                schema: "HR",
                table: "EmployeeAdvances",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "HR",
                table: "EmployeeAdvances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "EmployeeAdvances",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                schema: "HR",
                table: "EmployeeAdvances",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAdvances_CreatedById",
                schema: "HR",
                table: "EmployeeAdvances",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAdvances_UpdatedById",
                schema: "HR",
                table: "EmployeeAdvances",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAdvances_AspNetUsers_CreatedById",
                schema: "HR",
                table: "EmployeeAdvances",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAdvances_AspNetUsers_UpdatedById",
                schema: "HR",
                table: "EmployeeAdvances",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
