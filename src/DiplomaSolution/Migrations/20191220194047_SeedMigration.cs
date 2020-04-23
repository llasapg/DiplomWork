using Microsoft.EntityFrameworkCore.Migrations;

namespace DiplomaSolution.Migrations
{
    public partial class SeedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "EmailAddress", "FileId", "FirstName", "LastName", "Password" },
                values: new object[] { 10, "sss", 1, "Yebvhen", "Havrasiienko", "1223" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
