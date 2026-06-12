using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHashes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "AnnouncementId",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 6, 11, 20, 0, 59, 407, DateTimeKind.Utc).AddTicks(6717));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "AnnouncementId",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 6, 1, 20, 0, 59, 407, DateTimeKind.Utc).AddTicks(6720));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "AuditId",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 6, 6, 20, 0, 59, 407, DateTimeKind.Utc).AddTicks(6767));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "AuditId",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 6, 9, 20, 0, 59, 407, DateTimeKind.Utc).AddTicks(6770));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "AuditId",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 6, 7, 20, 0, 59, 407, DateTimeKind.Utc).AddTicks(6773));

            migrationBuilder.UpdateData(
                table: "Leaves",
                keyColumn: "LeaveId",
                keyValue: 1,
                column: "AppliedOn",
                value: new DateTime(2026, 6, 11, 20, 0, 59, 407, DateTimeKind.Utc).AddTicks(6576));

            migrationBuilder.UpdateData(
                table: "Leaves",
                keyColumn: "LeaveId",
                keyValue: 2,
                columns: new[] { "AppliedOn", "ApprovedOn" },
                values: new object[] { new DateTime(2026, 6, 11, 20, 0, 59, 407, DateTimeKind.Utc).AddTicks(6582), new DateTime(2026, 6, 11, 20, 0, 59, 407, DateTimeKind.Utc).AddTicks(6583) });

            migrationBuilder.UpdateData(
                table: "Leaves",
                keyColumn: "LeaveId",
                keyValue: 3,
                columns: new[] { "AppliedOn", "ApprovedOn" },
                values: new object[] { new DateTime(2026, 6, 11, 20, 0, 59, 407, DateTimeKind.Utc).AddTicks(6588), new DateTime(2026, 6, 11, 20, 0, 59, 407, DateTimeKind.Utc).AddTicks(6589) });

            migrationBuilder.UpdateData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$fBzVsc515Mpa25JqOKMX4uO6.cFT4Aqho9eRmtOzsaE6dIDCcQ9ay");

            migrationBuilder.UpdateData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$jShHrfEX3hmjHHBDWYkOuu7sRhIbvLKZk1VYVVsq8sLPK4RdEYEaW");

            migrationBuilder.UpdateData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$0YWVoOiAnhC5/1hSLKH1J.Jknbt6oqNKUDiagArnDHq4sQahPfXVK");

            migrationBuilder.UpdateData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 4,
                column: "PasswordHash",
                value: "$2a$11$0YWVoOiAnhC5/1hSLKH1J.Jknbt6oqNKUDiagArnDHq4sQahPfXVK");

            migrationBuilder.UpdateData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 5,
                column: "PasswordHash",
                value: "$2a$11$0YWVoOiAnhC5/1hSLKH1J.Jknbt6oqNKUDiagArnDHq4sQahPfXVK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "AnnouncementId",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 6, 11, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3540));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "AnnouncementId",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 6, 1, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3542));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "AuditId",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 6, 6, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3567));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "AuditId",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 6, 9, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3569));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "AuditId",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 6, 7, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3570));

            migrationBuilder.UpdateData(
                table: "Leaves",
                keyColumn: "LeaveId",
                keyValue: 1,
                column: "AppliedOn",
                value: new DateTime(2026, 6, 11, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3451));

            migrationBuilder.UpdateData(
                table: "Leaves",
                keyColumn: "LeaveId",
                keyValue: 2,
                columns: new[] { "AppliedOn", "ApprovedOn" },
                values: new object[] { new DateTime(2026, 6, 11, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3455), new DateTime(2026, 6, 11, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3456) });

            migrationBuilder.UpdateData(
                table: "Leaves",
                keyColumn: "LeaveId",
                keyValue: 3,
                columns: new[] { "AppliedOn", "ApprovedOn" },
                values: new object[] { new DateTime(2026, 6, 11, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3459), new DateTime(2026, 6, 11, 19, 33, 2, 397, DateTimeKind.Utc).AddTicks(3460) });

            migrationBuilder.UpdateData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$K1QlSqFCx9mhXRqFORqMQe3Y6zAFZ1KkH7Wg2mTPKJ5DzH4OkqBaS");

            migrationBuilder.UpdateData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$2dUPfx1fKQAmmdnWSchCteMO17QjRuMPuqx5vwJZk23fzLoIIWdzG");

            migrationBuilder.UpdateData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$smwho.ejCFLEx5Nljb7FwOonk5sYIb1llIZWEPPH/CUHOLPJa0miS");

            migrationBuilder.UpdateData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 4,
                column: "PasswordHash",
                value: "$2a$11$smwho.ejCFLEx5Nljb7FwOonk5sYIb1llIZWEPPH/CUHOLPJa0miS");

            migrationBuilder.UpdateData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 5,
                column: "PasswordHash",
                value: "$2a$11$smwho.ejCFLEx5Nljb7FwOonk5sYIb1llIZWEPPH/CUHOLPJa0miS");
        }
    }
}
