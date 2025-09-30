using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditStaffTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rewards",
                schema: "HR",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "VariableSalary",
                schema: "HR",
                table: "Staff");

            migrationBuilder.RenameColumn(
                name: "VacationDays",
                schema: "HR",
                table: "Staff",
                newName: "AnnualDays");

            migrationBuilder.AlterColumn<decimal>(
                name: "BasicSalary",
                schema: "HR",
                table: "Staff",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "BirthDate",
                schema: "HR",
                table: "Staff",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<decimal>(
                name: "Deductions",
                schema: "HR",
                table: "Staff",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NetSalary",
                schema: "HR",
                table: "Staff",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                schema: "HR",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "Deductions",
                schema: "HR",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "NetSalary",
                schema: "HR",
                table: "Staff");

            migrationBuilder.RenameColumn(
                name: "AnnualDays",
                schema: "HR",
                table: "Staff",
                newName: "VacationDays");

            migrationBuilder.AlterColumn<double>(
                name: "BasicSalary",
                schema: "HR",
                table: "Staff",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Rewards",
                schema: "HR",
                table: "Staff",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VariableSalary",
                schema: "HR",
                table: "Staff",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
