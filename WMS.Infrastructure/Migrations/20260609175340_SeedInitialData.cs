using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CreatedOn", "DepartmentName", "Description", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 9, 23, 23, 40, 277, DateTimeKind.Local).AddTicks(3426), "IT", "Information Technology", null },
                    { 2, new DateTime(2026, 6, 9, 23, 23, 40, 277, DateTimeKind.Local).AddTicks(3439), "HR", "Human Resources", null },
                    { 3, new DateTime(2026, 6, 9, 23, 23, 40, 277, DateTimeKind.Local).AddTicks(3440), "Finance", "Finance Department", null },
                    { 4, new DateTime(2026, 6, 9, 23, 23, 40, 277, DateTimeKind.Local).AddTicks(3442), "Operations", "Operations Department", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Description", "RoleName" },
                values: new object[,]
                {
                    { 1, "System Administrator", "Admin" },
                    { 2, "Department Manager", "Manager" },
                    { 3, "Regular Employee", "Employee" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3);
        }
    }
}
