using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEndToEndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Announcements",
                columns: new[] { "AnnouncementId", "CreatedBy", "CreatedOn", "IsActive", "Message", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 6, 11, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3540), true, "Welcome to the new Workforce Management System!", "Welcome to WMS" },
                    { 2, 1, new DateTime(2026, 6, 1, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3542), false, "The legacy system will be shut down next week.", "Legacy Shutdown" }
                });

            migrationBuilder.InsertData(
                table: "AuditLogs",
                columns: new[] { "AuditId", "Action", "CreatedBy", "CreatedOn", "EntityName", "RecordId" },
                values: new object[,]
                {
                    { 1, "Added", 1, new DateTime(2026, 6, 6, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3567), "Employee", 3 },
                    { 2, "Modified", 2, new DateTime(2026, 6, 9, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3569), "Leave", 2 },
                    { 3, "Added", 1, new DateTime(2026, 6, 7, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3570), "EmployeeProjectAllocation", 1 }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "ClientId", "ClientAddress", "ClientLocation", "ClientName", "ClientPhoneNumber", "Status" },
                values: new object[,]
                {
                    { 1, "123 Tech Lane", "New York", "TechCorp", 9999999991L, true },
                    { 2, "456 Finance Blvd", "London", "GlobalFinance", 9999999992L, true }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "CreatedOn", "DOB", "DOJ", "DepartmentId", "Email", "FirstName", "Gender", "LastName", "PhoneNumber", "RoleId", "Status", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "admin@wms.com", "System", "M", "Admin", "1111111111", 1, "Active", null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1985, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "manager@wms.com", "System", "F", "Manager", "2222222222", 2, "Active", null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "rahul@wms.com", "Rahul", "M", "Sharma", "3333333333", 3, "Active", null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1992, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "priya@wms.com", "Priya", "F", "Singh", "4444444444", 3, "Active", null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1994, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "amit@wms.com", "Amit", "M", "Kumar", "5555555555", 3, "Active", null }
                });

            migrationBuilder.UpdateData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 3,
                column: "Username",
                value: "rahul");

            migrationBuilder.InsertData(
                table: "UserLogins",
                columns: new[] { "UserId", "LastLogin", "PasswordHash", "RoleId", "Username" },
                values: new object[,]
                {
                    { 4, null, "$2a$11$smwho.ejCFLEx5Nljb7FwOonk5sYIb1llIZWEPPH/CUHOLPJa0miS", 3, "priya" },
                    { 5, null, "$2a$11$smwho.ejCFLEx5Nljb7FwOonk5sYIb1llIZWEPPH/CUHOLPJa0miS", 3, "amit" }
                });

            migrationBuilder.InsertData(
                table: "Attendances",
                columns: new[] { "AttendanceId", "AttendanceDate", "CheckIn", "CheckOut", "EmpId", "TotalHours", "WorkMode" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 12, 9, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 12, 17, 0, 0, 0, DateTimeKind.Local), 3, 8.0, "Office" },
                    { 2, new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 12, 9, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 12, 17, 0, 0, 0, DateTimeKind.Local), 4, 8.0, "Remote" },
                    { 3, new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 12, 9, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 12, 17, 0, 0, 0, DateTimeKind.Local), 5, 8.0, "Office" },
                    { 4, new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 11, 9, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 11, 17, 0, 0, 0, DateTimeKind.Local), 3, 8.0, "Office" },
                    { 5, new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 11, 9, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 11, 17, 0, 0, 0, DateTimeKind.Local), 4, 8.0, "Remote" },
                    { 6, new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 11, 9, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 11, 17, 0, 0, 0, DateTimeKind.Local), 5, 8.0, "Office" },
                    { 7, new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 10, 9, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 10, 17, 0, 0, 0, DateTimeKind.Local), 3, 8.0, "Office" },
                    { 8, new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 10, 9, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 10, 17, 0, 0, 0, DateTimeKind.Local), 4, 8.0, "Remote" },
                    { 9, new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 10, 9, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 10, 17, 0, 0, 0, DateTimeKind.Local), 5, 8.0, "Office" },
                    { 10, new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 9, 9, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 9, 17, 0, 0, 0, DateTimeKind.Local), 3, 8.0, "Office" },
                    { 11, new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 9, 9, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 9, 17, 0, 0, 0, DateTimeKind.Local), 4, 8.0, "Remote" },
                    { 12, new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 9, 9, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 9, 17, 0, 0, 0, DateTimeKind.Local), 5, 8.0, "Office" },
                    { 13, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 8, 9, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 8, 17, 0, 0, 0, DateTimeKind.Local), 3, 8.0, "Office" },
                    { 14, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 8, 9, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 8, 17, 0, 0, 0, DateTimeKind.Local), 4, 8.0, "Remote" },
                    { 15, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 8, 9, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 6, 8, 17, 0, 0, 0, DateTimeKind.Local), 5, 8.0, "Office" }
                });

            migrationBuilder.InsertData(
                table: "Leaves",
                columns: new[] { "LeaveId", "AppliedOn", "ApprovedBy", "ApprovedOn", "EmpId", "FromDate", "LeaveType", "Reason", "Status", "ToDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 11, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3451), null, null, 3, new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Local), "Sick", "Fever", "Pending", new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Local) },
                    { 2, new DateTime(2026, 6, 11, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3455), 2, new DateTime(2026, 6, 11, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3456), 4, new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Local), "Vacation", "Trip", "Approved", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Local) },
                    { 3, new DateTime(2026, 6, 11, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3459), 2, new DateTime(2026, 6, 11, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3460), 5, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Local), "Casual", "Family event", "Rejected", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Local) }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "ProjectId", "ClientId", "EndDate", "ProjectName", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "WMS Project", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { 2, 1, new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR Portal", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inactive" },
                    { 3, 2, new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finance Dashboard", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" }
                });

            migrationBuilder.InsertData(
                table: "EmployeeProjectAllocations",
                columns: new[] { "AllocationId", "AssignedOn", "CreateDate", "CreatedBy", "EmpId", "ProjectId", "Status", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 3, 1, true, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 4, 2, false, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 5, 3, true, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Announcements",
                keyColumn: "AnnouncementId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Announcements",
                keyColumn: "AnnouncementId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AuditLogs",
                keyColumn: "AuditId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AuditLogs",
                keyColumn: "AuditId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AuditLogs",
                keyColumn: "AuditId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EmployeeProjectAllocations",
                keyColumn: "AllocationId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EmployeeProjectAllocations",
                keyColumn: "AllocationId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EmployeeProjectAllocations",
                keyColumn: "AllocationId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Leaves",
                keyColumn: "LeaveId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Leaves",
                keyColumn: "LeaveId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Leaves",
                keyColumn: "LeaveId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "ProjectId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "ProjectId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "ProjectId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 3,
                column: "Username",
                value: "employee");
        }
    }
}
