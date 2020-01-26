using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiplomaSolution.Migrations
{
    public partial class ChangingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 85,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 85,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 85);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IdentityUser",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 85, nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 85, nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 85, nullable: true),
                    EmailConfirmed = table.Column<int>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<int>(nullable: false),
                    TwoFactorEnabled = table.Column<int>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<int>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                maxLength: 85,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                maxLength: 85,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                maxLength: 85,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
