using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddManagerAndEmployeeSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserLogins",
                columns: new[] { "UserId", "LastLogin", "PasswordHash", "RoleId", "Username" },
                values: new object[,]
                {
                    { 2, null, "$2a$11$2dUPfx1fKQAmmdnWSchCteMO17QjRuMPuqx5vwJZk23fzLoIIWdzG", 2, "manager" },
                    { 3, null, "$2a$11$smwho.ejCFLEx5Nljb7FwOonk5sYIb1llIZWEPPH/CUHOLPJa0miS", 3, "employee" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserLogins",
                keyColumn: "UserId",
                keyValue: 3);
        }
    }
}
