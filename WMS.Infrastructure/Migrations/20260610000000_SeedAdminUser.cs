using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserLogins",
                columns: new[] { "UserId", "Username", "PasswordHash", "RoleId", "LastLogin" },
                values: new object[]
                {
                    1,
                    "admin",
                    "$2a$11$K1QlSqFCx9mhXRqFORqMQe3Y6zAFZ1KkH7Wg2mTPKJ5DzH4OkqBaS",
                    1,
                    null
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 1);
        }
    }
}
