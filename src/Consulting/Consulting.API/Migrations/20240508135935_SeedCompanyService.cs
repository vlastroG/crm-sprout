using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consulting.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedCompanyService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CompanyServices",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 1, "Консультации пользователей и администраторов систем", "Техническая поддержка" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CompanyServices",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
