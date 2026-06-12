using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixLeaveApprovedByForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProjectAllocations_Employees_EmpId",
                table: "EmployeeProjectAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProjectAllocations_Projects_ProjectId",
                table: "EmployeeProjectAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Employees_ApprovedBy",
                table: "Leaves");

            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Employees_EmpId",
                table: "Leaves");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));



            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CreatedBy",
                table: "Announcements",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProjectAllocations_Employees_EmpId",
                table: "EmployeeProjectAllocations",
                column: "EmpId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProjectAllocations_Projects_ProjectId",
                table: "EmployeeProjectAllocations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Employees_EmpId",
                table: "Leaves",
                column: "EmpId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_UserLogins_ApprovedBy",
                table: "Leaves",
                column: "ApprovedBy",
                principalTable: "UserLogins",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProjectAllocations_Employees_EmpId",
                table: "EmployeeProjectAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProjectAllocations_Projects_ProjectId",
                table: "EmployeeProjectAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Employees_EmpId",
                table: "Leaves");

            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_UserLogins_ApprovedBy",
                table: "Leaves");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_CreatedBy",
                table: "Announcements");



            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 6, 9, 23, 23, 40, 277, DateTimeKind.Local).AddTicks(3426));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 6, 9, 23, 23, 40, 277, DateTimeKind.Local).AddTicks(3439));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 6, 9, 23, 23, 40, 277, DateTimeKind.Local).AddTicks(3440));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2026, 6, 9, 23, 23, 40, 277, DateTimeKind.Local).AddTicks(3442));

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProjectAllocations_Employees_EmpId",
                table: "EmployeeProjectAllocations",
                column: "EmpId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProjectAllocations_Projects_ProjectId",
                table: "EmployeeProjectAllocations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Employees_ApprovedBy",
                table: "Leaves",
                column: "ApprovedBy",
                principalTable: "Employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Employees_EmpId",
                table: "Leaves",
                column: "EmpId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
