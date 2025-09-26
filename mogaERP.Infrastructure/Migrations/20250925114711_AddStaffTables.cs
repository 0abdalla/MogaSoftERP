using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mogaERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MasterData");

            migrationBuilder.CreateTable(
                name: "Branches",
                schema: "MasterData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Branches_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobLevels",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobLevels_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobLevels_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobTitles",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_JobTitles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTitles_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobTitles_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobTitles_JobDepartments_JobDepartmentId",
                        column: x => x.JobDepartmentId,
                        principalSchema: "HR",
                        principalTable: "JobDepartments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobTypes",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTypes_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobTypes_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MaritalStatus = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    JobTitleId = table.Column<int>(type: "int", nullable: true),
                    JobTypeId = table.Column<int>(type: "int", nullable: true),
                    JobLevelId = table.Column<int>(type: "int", nullable: true),
                    JobDepartmentId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    BasicSalary = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: true),
                    Tax = table.Column<int>(type: "int", nullable: true),
                    Insurance = table.Column<int>(type: "int", nullable: true),
                    VacationDays = table.Column<int>(type: "int", nullable: true),
                    IsAuthorized = table.Column<bool>(type: "bit", nullable: false),
                    VariableSalary = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    VisaCode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Allowances = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Rewards = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staff_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staff_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Staff_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "MasterData",
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Staff_JobDepartments_JobDepartmentId",
                        column: x => x.JobDepartmentId,
                        principalSchema: "HR",
                        principalTable: "JobDepartments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Staff_JobLevels_JobLevelId",
                        column: x => x.JobLevelId,
                        principalSchema: "HR",
                        principalTable: "JobLevels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Staff_JobTitles_JobTitleId",
                        column: x => x.JobTitleId,
                        principalSchema: "HR",
                        principalTable: "JobTitles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Staff_JobTypes_JobTypeId",
                        column: x => x.JobTypeId,
                        principalSchema: "HR",
                        principalTable: "JobTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AttendanceSalaries",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    WorkHours = table.Column<int>(type: "int", nullable: true),
                    WorkDays = table.Column<double>(type: "float(10)", precision: 10, scale: 2, nullable: true),
                    RequiredHours = table.Column<int>(type: "int", nullable: true),
                    TotalFingerprintHours = table.Column<double>(type: "float(10)", precision: 10, scale: 2, nullable: true),
                    SickDays = table.Column<double>(type: "float(10)", precision: 10, scale: 2, nullable: true),
                    OtherDays = table.Column<double>(type: "float(10)", precision: 10, scale: 2, nullable: true),
                    Fridays = table.Column<int>(type: "int", nullable: true),
                    TotalDays = table.Column<int>(type: "int", nullable: true),
                    Overtime = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceSalaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceSalaries_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttendanceSalaries_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AttendanceSalaries_Staff_StaffId",
                        column: x => x.StaffId,
                        principalSchema: "HR",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAdvances",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvanceNumber = table.Column<int>(type: "int", nullable: false),
                    AdvanceTypeId = table.Column<int>(type: "int", nullable: false),
                    AdvanceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AdvanceAmount = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    PaymentAmount = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    Benefit = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: true),
                    PaymentFromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkflowStatusId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAdvances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAdvances_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeAdvances_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeAdvances_Staff_StaffId",
                        column: x => x.StaffId,
                        principalSchema: "HR",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffAttachments",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffAttachments_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffAttachments_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StaffAttachments_Staff_StaffId",
                        column: x => x.StaffId,
                        principalSchema: "HR",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceSalaries_CreatedById",
                schema: "HR",
                table: "AttendanceSalaries",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceSalaries_StaffId",
                schema: "HR",
                table: "AttendanceSalaries",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceSalaries_UpdatedById",
                schema: "HR",
                table: "AttendanceSalaries",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CreatedById",
                schema: "MasterData",
                table: "Branches",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_UpdatedById",
                schema: "MasterData",
                table: "Branches",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAdvances_CreatedById",
                schema: "HR",
                table: "EmployeeAdvances",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAdvances_StaffId",
                schema: "HR",
                table: "EmployeeAdvances",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAdvances_UpdatedById",
                schema: "HR",
                table: "EmployeeAdvances",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobLevels_CreatedById",
                schema: "HR",
                table: "JobLevels",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobLevels_UpdatedById",
                schema: "HR",
                table: "JobLevels",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_CreatedById",
                schema: "HR",
                table: "JobTitles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_JobDepartmentId",
                schema: "HR",
                table: "JobTitles",
                column: "JobDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_UpdatedById",
                schema: "HR",
                table: "JobTitles",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobTypes_CreatedById",
                schema: "HR",
                table: "JobTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobTypes_UpdatedById",
                schema: "HR",
                table: "JobTypes",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_BranchId",
                schema: "HR",
                table: "Staff",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_CreatedById",
                schema: "HR",
                table: "Staff",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_JobDepartmentId",
                schema: "HR",
                table: "Staff",
                column: "JobDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_JobLevelId",
                schema: "HR",
                table: "Staff",
                column: "JobLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_JobTitleId",
                schema: "HR",
                table: "Staff",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_JobTypeId",
                schema: "HR",
                table: "Staff",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_UpdatedById",
                schema: "HR",
                table: "Staff",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StaffAttachments_CreatedById",
                schema: "HR",
                table: "StaffAttachments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StaffAttachments_StaffId",
                schema: "HR",
                table: "StaffAttachments",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffAttachments_UpdatedById",
                schema: "HR",
                table: "StaffAttachments",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceSalaries",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "EmployeeAdvances",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "StaffAttachments",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "Staff",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "Branches",
                schema: "MasterData");

            migrationBuilder.DropTable(
                name: "JobLevels",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "JobTitles",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "JobTypes",
                schema: "HR");
        }
    }
}
